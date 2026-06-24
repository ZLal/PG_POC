namespace PaymentGatewayPOC.Models;

public class Transaction
{
    public Guid TransactionId { get; set; }

    public Guid ApplicationId { get; set; }

    public Guid GatewayId { get; set; }

    public decimal Amount { get; set; }

    public TransactionStatus Status { get; set; } = TransactionStatus.Pending;

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    public DateTime LastUpdatedDate { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public Application? Application { get; set; }

    public Gateway? Gateway { get; set; }

    public ICollection<TransactionDetail> TransactionDetails { get; set; } = [];
}