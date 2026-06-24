namespace PaymentGatewayPOC.Models;

public class TransactionDetail
{
    public Guid TransactionDetailId { get; set; }

    public Guid TransactionId { get; set; }

    public string Status { get; set; } = string.Empty;

    public string? Message { get; set; }

    public string? Data { get; set; }

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public Transaction? Transaction { get; set; }
}