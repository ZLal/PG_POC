using PaymentGatewayPOC.Models;

namespace PaymentGatewayPOC.Payments.Interfaces;

public interface IPaymentGatewayProcessor
{
    string Name { get; }
    Version Version { get; }

    Task<IList<KeyValuePair<string, string>>> StartPaymentProcess(Gateway gateway, Transaction transaction);
    Task VerifyDataAsync(Transaction transaction, IList<KeyValuePair<string, string>> paymentData);
    Task<TransactionStatus> GetTransactionStatusFromDataAsync(string updateEvent, IList<KeyValuePair<string, string>> paymentData);
}