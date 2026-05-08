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
    [MaxLength(255)]
    public string ClientId { get; set; } = string.Empty;

    [Required]
    [MaxLength(500)]
    public string AccessLocation { get; set; } = string.Empty;

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    // Navigation properties
    [ForeignKey("OrganizationId")]
    public Organization? Organization { get; set; }

    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}