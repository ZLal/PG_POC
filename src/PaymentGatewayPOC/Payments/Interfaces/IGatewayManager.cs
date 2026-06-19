using PaymentGatewayPOC.Models;

namespace PaymentGatewayPOC.Payments.Interfaces;

public interface IGatewayManager
{
    Task<Guid> CreateTransactionAsync(Guid applicationId, Guid clientId, string clientSecret, decimal amount);
    Task StartPaymentProcessAsync(Guid transactionId);
    Task UpdateUserAuthorizationAsync(Guid transactionId, IList<KeyValuePair<string, string>> paymentData);
    Task<TransactionStatus> GetPaymentStatusAsync(Guid transactionId);
}
