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
    [MaxLength(50)]
    public string Status { get; set; } = "Active";

    // Navigation properties
    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}