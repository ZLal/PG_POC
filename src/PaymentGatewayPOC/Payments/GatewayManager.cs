using System.Text.Json;
using PaymentGatewayPOC.Models;
using PaymentGatewayPOC.Services.Interfaces;
using PaymentGatewayPOC.Payments.Interfaces;

namespace PaymentGatewayPOC.Payments;

public class GatewayManager(ILogger<GatewayManager> logger, IGatewayService gatewayService,
    List<IPaymentGatewayProcessor> paymentGateways,
    IApplicationService applicationService,
    IClientService clientService,
    ITransactionService transactionService) : IGatewayManager
{
    private IPaymentGatewayProcessor GetPaymentGatewayProcessor(Gateway gateway)
    {
        IPaymentGatewayProcessor paymentGateway = paymentGateways.SingleOrDefault(x => x.Name == gateway.Name)
            ?? throw new Exception($"Failed to load payment gateway. No gateway with name {gateway.Name} available");
        logger.LogInformation("Payment gateway {name} selected", paymentGateway.Name);
        return paymentGateway;
    }

    public async Task<Guid> CreateTransactionAsync(Guid applicationId, Guid clientId, string clientSecret, decimal amount)
    {
        Client? client = await clientService.GetClientBySecretKeyAsync(clientId, clientSecret);
        if (client == null || applicationId != client.ApplicationId)
        {
            throw new Exception("Invalid client credentials or application mismatch.");
        }

        Application? application = await applicationService.GetApplicationWithActiveGatewayAsync(client.ApplicationId);
        if (application == null || application.ApplicationGateways == null)
        {
            throw new Exception("Application not found or no active gateway available.");
        }

        // TODO Implement gateway selection logic
        Guid gatewayId = application.ApplicationGateways.First().GatewayId;

        Transaction transaction = await transactionService.CreateTransactionAsync(
                application.ApplicationId,
                gatewayId,
                amount
            );
        return transaction.TransactionId;
    }

    public async Task StartPaymentProcessAsync(Guid transactionId)
    {
        Transaction? transaction = await transactionService.GetTransactionByIdAsync(transactionId);
        if (transaction == null || transaction.Status != TransactionStatus.Pending)
        {
            throw new Exception("Transaction not found or not in pending status.");
        }
        Gateway gateway = await gatewayService.GetGatewayByIdWithGatewayDetailsAsync(transaction.GatewayId)
            ?? throw new Exception($"Invalid gateway id {transaction.GatewayId}");
        
        var paymentGateway = GetPaymentGatewayProcessor(gateway);
        var startPaymentResult = await paymentGateway.StartPaymentProcess(gateway, transaction);
        transaction.Status = await paymentGateway.GetTransactionStatusFromDataAsync("OrderCreate", startPaymentResult);
        await UpdatePaymentStatusAsync(transaction, startPaymentResult);
    }

    public async Task UpdateUserAuthorizationAsync(Guid transactionId, IList<KeyValuePair<string, string>> paymentData)
    {
        Transaction transaction = await transactionService.GetTransactionByIdAsync(transactionId)
            ?? throw new Exception("Transaction not found");
        if (transaction.Status != TransactionStatus.InPayment)
        {
            throw new Exception("Transaction not found or not in payment status.");
        }
        Gateway gateway = await gatewayService.GetGatewayByIdAsync(transaction.GatewayId)
            ?? throw new Exception($"Invalid gateway id {transaction.GatewayId}");
        
        var paymentGateway = GetPaymentGatewayProcessor(gateway);
        Exception? verificationException = null;
        try
        {
            await paymentGateway.VerifyDataAsync(transaction, paymentData);
        }
        catch(Exception ex)
        {
            verificationException = ex;
            logger.LogError(ex, "Payment data verification failed in UpdateUserAuthorization");
        }
        if (verificationException == null)
        {
            transaction.Status = TransactionStatus.Verified;
        }
        else
        {
            transaction.Status = TransactionStatus.Failed;
            _ = await transactionService.AddErrorLogAsync(transaction.TransactionId, verificationException.Message);
        }
        await UpdatePaymentStatusAsync(transaction, paymentData);
    }

    private async Task UpdatePaymentStatusAsync(Transaction transaction, IList<KeyValuePair<string, string>> paymentData)
    {
        // TODO Check what happens when null
        string status = paymentData.FirstOrDefault(x => x.Key == "Status").Value ?? "Unspecified";
        string message = paymentData.FirstOrDefault(x => x.Key == "Message").Value ?? "Unspecified";

        string paymentDataStr = JsonSerializer.Serialize(paymentData);
        TransactionDetail transactionDetail = new()
        {
            TransactionDetailId = Guid.Empty,
            TransactionId = transaction.TransactionId,
            Status = status,
            Message = message,
            Data = paymentDataStr,
            CreatedDate = DateTime.UtcNow
        };
        _ = await transactionService.AddTransactionDetailAsync(transactionDetail);
        _ = await transactionService.UpdateTransactionStatusAsync(transaction.TransactionId, transactionDetail.CreatedDate, transaction.Status);
    }

    public async Task<TransactionStatus> GetPaymentStatusAsync(Guid transactionId)
    {
        Transaction transaction = await transactionService.GetTransactionByIdAsync(transactionId)
            ?? throw new Exception($"Invalid transaction id {transactionId}");
        return transaction.Status;
    }
}
