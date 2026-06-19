using PaymentGatewayPOC.Models;
using PaymentGatewayPOC.Payments.Interfaces;

namespace PaymentGatewayPOC.Payments.Gateways;

public class Gateway_Dummy(ILogger<Gateway_Dummy> logger) : IPaymentGatewayProcessor
{
    public const string DefaultCurrency = "INR";

    public string Name => "DummyPay";
    public Version Version => new(0, 1, 0);

    public Task<IList<KeyValuePair<string, string>>> StartPaymentProcess(Gateway gateway, Transaction transaction)
    {
        IList<KeyValuePair<string, string>> result = [];
        logger.LogInformation("Starting payment process in {name} gateway", Name);
        return Task.FromResult(result);
    }

    public Task VerifyDataAsync(Transaction transaction, IList<KeyValuePair<string, string>> paymentData)
    {
        logger.LogInformation("Data verification in {name} gateway", Name);
        return Task.FromResult(true);
    }

    public Task<TransactionStatus> GetTransactionStatusFromDataAsync(string updateEvent, IList<KeyValuePair<string, string>> paymentData)
    {
        logger.LogInformation("Getting transaction status from data in {name} gateway", Name);
        string status = paymentData.FirstOrDefault(x => x.Key == "Status").Value ?? "Unspecified";
        bool parseStatus = Enum.TryParse(status, out TransactionStatus transactionStatus);
        if (parseStatus == false)
            throw new Exception("Failed to parse payment status");
        return Task.FromResult(transactionStatus);
    }
}
