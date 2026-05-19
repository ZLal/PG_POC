using System.ComponentModel.DataAnnotations;

namespace PaymentGatewayPOC.Models;

public class Gateway
{
    [Key]
    public Guid GatewayId { get; set; }

    [Required]
    [MaxLength(255)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public GatewayStatus Status { get; set; } = GatewayStatus.Active;

    // Navigation properties
    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    public ICollection<ApplicationGateway> ApplicationGateways { get; set; } = new List<ApplicationGateway>();
}