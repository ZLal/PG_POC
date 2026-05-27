namespace PaymentGatewayPOC.Models;

public class ErrorLog
{
    public Guid LogId { get; set; }

    public Guid? TransactionId { get; set; }

    public string ErrorMessage { get; set; } = string.Empty;

    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}