namespace PaymentGatewayPOC.Models;

public enum TransactionStatus
{
    Pending,
    InPayment,
    Paid,
    Cancelled,
    Refunded,
    Failed,
    Error
}