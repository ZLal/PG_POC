namespace PaymentGatewayPOC.Models;

public enum TransactionStatus
{
    Pending,
    InPayment,
    Paid,
    Refunded,
    Failed,
    Error
}