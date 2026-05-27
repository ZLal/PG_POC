namespace PaymentGatewayPOC.Models;

public class Application
{
    public Guid ApplicationId { get; set; }

    public Guid OrganizationId { get; set; }

    public string Name { get; set; } = string.Empty;

    public Guid ClientId { get; set; }

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public Organization? Organization { get; set; }

    public ICollection<Transaction> Transactions { get; set; } = [];
    public ICollection<Client> Clients { get; set; } = [];
    public ICollection<ApplicationGateway> ApplicationGateways { get; set; } = [];
}