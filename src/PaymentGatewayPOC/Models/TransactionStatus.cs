namespace PaymentGatewayPOC.Models;

public enum TransactionStatus
{
    Pending,
    InPayment,
    Verified,
    Paid,
    Cancelled,
    Refunded,
    Failed,
    Error
}