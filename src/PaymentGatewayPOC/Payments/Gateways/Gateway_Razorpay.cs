using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Razorpay.Api; // Official Razorpay namespace
using PaymentGatewayPOC.Models;
using PaymentGatewayPOC.Payments.Interfaces;
using System.Net;

namespace PaymentGatewayPOC.Payments.Gateways;

// Ref: https://razorpay.com/docs/payments/server-integration/dot-net/integration-steps/

public class Gateway_Razorpay(ILogger<Gateway_Razorpay> logger) : IPaymentGatewayProcessor
{
    public const string DefaultCurrency = "INR";

    public string Name => "Razorpay";
    public Version Version => new(0, 1, 0);

    public Task<IList<KeyValuePair<string, string>>> StartPaymentProcess(Gateway gateway, Transaction transaction)
    {
        logger.LogInformation("Starting payment process in {name} gateway", Name);
        // TODO Fix this
        throw new NotImplementedException();
    }

    public string? GetValueByKey(IList<KeyValuePair<string, string>> data, string key)
    {
        return data.SingleOrDefault(x => string.Equals(x.Key, key, StringComparison.InvariantCultureIgnoreCase)).Value;
    }

    public Task VerifyDataAsync(Transaction transaction, IList<KeyValuePair<string, string>> paymentData)
    {
        logger.LogInformation("Data verification in {name} gateway", Name);
        string paymentId = GetValueByKey(paymentData, nameof(paymentId))
            ?? throw new Exception($"Failed to load {nameof(paymentId)} from paymentData");
        string orderId = GetValueByKey(paymentData, nameof(orderId))
            ?? throw new Exception($"Failed to load {nameof(orderId)} from paymentData");
        string secret = GetValueByKey(paymentData, nameof(secret))
            ?? throw new Exception($"Failed to load {nameof(secret)} from paymentData");
        VerifyPayment(paymentId, orderId, secret);
        return Task.CompletedTask;
    }

    public Task<TransactionStatus> GetTransactionStatusFromDataAsync(string updateEvent, IList<KeyValuePair<string, string>> paymentData)
    {
        logger.LogInformation("Getting transaction status from data in {name} gateway", Name);
        //string status = paymentData.FirstOrDefault(x => x.Key == "Status").Value ?? "Unspecified";
        string status = GetValueByKey(paymentData, nameof(status)) ?? "Unspecified";
        TransactionStatus transactionStatus = status switch
        {
            "created" => TransactionStatus.InPayment,
            "authorized" => TransactionStatus.InPayment,
            "captured" => TransactionStatus.Paid,
            "refunded" => TransactionStatus.Refunded,
            "failed" => TransactionStatus.Failed,
            _ => TransactionStatus.Error
        };
        return Task.FromResult(transactionStatus);
    }

    // public void CreateOrder(string key, string secret, string orderId, decimal amount, string currency = DefaultCurrency)
    // {
    //     try
    //     {
    //         // Initialize Razorpay client
    //         RazorpayClient client = new (key, secret);

    //         // Amount in paise
    //         long transactionAmount = (long)Math.Round(amount * 100, 2);

    //         // Create order options
    //         Dictionary<string, object> options = new()
    //         {
    //             { "amount", transactionAmount },
    //             { "currency", currency },
    //             { "receipt", orderId },
    //             { "payment_capture", 1 } // Auto-capture
    //         };

    //         // Create order
    //         Order order = client.Order.Create(options);

    //         logger.LogInformation("Payment order created in {name} with Order ID: {OrderId}", Name, orderId);
    //     }
    //     catch (Exception ex)
    //     {
    //         logger.LogError(ex, "Error creating razorpay order");
    //         throw;
    //     }
    // }

    // public static bool VerifySignature(string orderId, string paymentId, string signature, string secret)
    // {
    //     string payload = orderId + "|" + paymentId;
    //     string expectedSignature;

    //     using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secret)))
    //     {
    //         byte[] hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(payload));
    //         // TODO Remove after testing
    //         // expectedSignature = BitConverter.ToString(hash).Replace("-", "").ToLower();
    //         expectedSignature = Convert.ToHexStringLower(hash);
    //     }

    //     return expectedSignature == signature;
    // }

    // -------------------------------------------------------------------

    private RazorpayClient CreateClient(string key, string secret)
    {
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        return new RazorpayClient(key, secret);
    }

    public string CreateOrder(string key, string secret, string orderId, decimal amount, string currency = DefaultCurrency)
    {
        try
        {
            long transactionAmount = (long)Math.Round(amount * 100, 2);
            Dictionary<string, object> input = new()
            {
                { "amount", transactionAmount }, // this amount should be same as transaction amount
                { "currency", currency },
                { "receipt", orderId },
                { "payment_capture", 1 }
            };
            var client = CreateClient(key, secret);
            Order order = client.Order.Create(input);
            logger.LogInformation("Payment order created in {name} with Order ID: {OrderId}", Name, orderId);
            return order["id"].ToString();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating razorpay order");
            throw;
        }
    }

    public void VerifyPayment(string paymentId, string orderId, string signature)
    {
        Dictionary<string, string> attributes = new()
        {
            { "razorpay_payment_id", paymentId },
            { "razorpay_order_id", orderId },
            { "razorpay_signature", signature }
        };
        Utils.verifyPaymentSignature(attributes);
    }

    public PaymentDetails GetPaymentByOrder(string key, string secret, string paymentOrderId)
    {
        var client = CreateClient(key, secret);
        List<Payment> payments = client.Order.Fetch(paymentOrderId).Payments();
        string paymentDetailsStr = payments[0].Attributes.ToString();
        var paymentDetails = JsonSerializer.Deserialize<PaymentDetails>(paymentDetailsStr)
            ?? throw new Exception($"Failed to deserialize PaymentDetails in {Name} with PaymentOrderID: {paymentOrderId}");
        return paymentDetails;
    }

    private static readonly DateTime Date1970 = new(1970, 1, 1);
    private static int ToUnixTimestamp(DateTime dateTime) => (int)dateTime.Subtract(Date1970).TotalSeconds;

    public List<PaymentDetails> GetPaymentList(string key, string secret, DateTime fromDate, DateTime toDate)
    {
        List<PaymentDetails> result = [];
        Dictionary <string, object> options = new()
        {
            //supported option filters (from, to, count, skip)
            { "from", ToUnixTimestamp(fromDate) },
            { "to", ToUnixTimestamp(toDate) },
            { "count", 100 }
            //{ "skip", 1000 }
        };

        var client = CreateClient(key, secret);
        List<Payment> payments = client.Payment.All(options);
        foreach (var payment in payments)
        {
            string paymentDetailsStr = payment.Attributes.ToString();
            var paymentDetails = JsonSerializer.Deserialize<PaymentDetails>(paymentDetailsStr)
                ?? throw new Exception($"Failed to deserialize PaymentList in {Name} with date between {fromDate} and {toDate}");
            result.Add(paymentDetails);
        }
        return result;
    }
}

#region PaymentDetails
public class PaymentDetails
{
    public string id { get; set; } = string.Empty;
    public string entity { get; set; } = string.Empty;
    public int amount { get; set; }
    public string currency { get; set; } = string.Empty;
    public string status { get; set; } = string.Empty;
    public string order_id { get; set; } = string.Empty;
    public object invoice_id { get; set; } = null!;
    public bool international { get; set; }
    public string method { get; set; } = string.Empty;
    public int? amount_refunded { get; set; }
    public object refund_status { get; set; } = string.Empty;
    public bool captured { get; set; }
    public string description { get; set; } = string.Empty;
    public object card_id { get; set; } = string.Empty;
    public object bank { get; set; } = string.Empty;
    public object wallet { get; set; } = string.Empty;
    public string vpa { get; set; } = string.Empty;
    public string email { get; set; } = string.Empty;
    public string contact { get; set; } = string.Empty;
    public Notes notes { get; set; } = null!;
    public int? fee { get; set; }
    public int? tax { get; set; }
    public object error_code { get; set; } = string.Empty;
    public object error_description { get; set; } = string.Empty;
    public object error_source { get; set; } = string.Empty;
    public object error_step { get; set; } = string.Empty;
    public object error_reason { get; set; } = string.Empty;
    public Acquirer_Data acquirer_data { get; set; } = null!;
    public int created_at { get; set; }
}

public class Notes
{
    public string address { get; set; } = string.Empty;
    public string merchant_order_id { get; set; } = string.Empty;
}

public class Acquirer_Data
{
    public string rrn { get; set; } = string.Empty;
}
#endregion


/*
using System;
using System.Collections.Generic;
using Razorpay.Api; // Official Razorpay namespace

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Replace with your Test or Live API keys
            string key = "rzp_test_YourKeyId";
            string secret = "YourKeySecret";

            // Initialize Razorpay client
            RazorpayClient client = new RazorpayClient(key, secret);

            // Create order options
            Dictionary<string, object> options = new Dictionary<string, object>
            {
                { "amount", 50000 }, // Amount in paise (50000 = ₹500)
                { "currency", "INR" },
                { "receipt", "order_rcptid_11" },
                { "payment_capture", 1 } // Auto-capture
            };

            // Create order
            Order order = client.Order.Create(options);

            Console.WriteLine("Order created successfully!");
            Console.WriteLine("Order ID: " + order["id"]);
            Console.WriteLine("Amount: " + order["amount"]);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error creating order: " + ex.Message);
        }
    }
}

*/