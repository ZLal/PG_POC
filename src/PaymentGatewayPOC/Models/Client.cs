using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaymentGatewayPOC.Models;

public class Client
{
    [Key]
    public Guid ClientId { get; set; }

    [Required]
    public Guid OrganizationId { get; set; }

    [Required]
    public Guid ApplicationId { get; set; }

    [Required]
    [MaxLength(255)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string SecretKey { get; set; } = string.Empty;

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    public DateTime? ExpiryDate { get; set; }

    // Navigation properties
    [ForeignKey("OrganizationId")]
    public Organization? Organization { get; set; }

    [ForeignKey("ApplicationId")]
    public Application? Application { get; set; }
}
