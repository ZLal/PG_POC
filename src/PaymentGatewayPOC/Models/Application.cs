using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaymentGatewayPOC.Models;

public class Application
{
    [Key]
    public Guid ApplicationId { get; set; }

    [Required]
    public Guid OrganizationId { get; set; }

    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;

    public Guid ClientId { get; set; }

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    // Navigation properties
    [ForeignKey("OrganizationId")]
    public Organization? Organization { get; set; }

    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    public ICollection<Client> Clients { get; set; } = new List<Client>();
    public ICollection<ApplicationGateway> ApplicationGateways { get; set; } = new List<ApplicationGateway>();
}