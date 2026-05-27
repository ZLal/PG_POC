namespace PaymentGatewayPOC.Models;

public class Client
{
    public Guid ClientId { get; set; }

    public Guid OrganizationId { get; set; }

    public Guid ApplicationId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string SecretKey { get; set; } = string.Empty;

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    public DateTime? ExpiryDate { get; set; }

    // Navigation properties
    public Organization? Organization { get; set; }

    public Application? Application { get; set; }
}
