namespace PaymentGatewayPOC.Models;

public class Gateway
{
    public Guid GatewayId { get; set; }

    public string Name { get; set; } = string.Empty;

    public GatewayStatus Status { get; set; } = GatewayStatus.Active;

    // Navigation properties
    public ICollection<Transaction> Transactions { get; set; } = [];
    public ICollection<ApplicationGateway> ApplicationGateways { get; set; } = [];
}