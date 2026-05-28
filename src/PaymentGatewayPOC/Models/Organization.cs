namespace PaymentGatewayPOC.Models;

public class Organization
{
    public Guid OrganizationId { get; set; }

    public string Name { get; set; } = string.Empty;

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public ICollection<Application> Applications { get; set; } = [];
}