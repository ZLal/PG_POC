using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaymentGatewayPOC.Models;

public class TransactionDetail
{
    [Key]
    public Guid TransactionDetailId { get; set; }

    public Guid? TransactionId { get; set; }

    [Required]
    [MaxLength(50)]
    public string Status { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string? Message { get; set; }

    public string? Data { get; set; }

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    // Navigation properties
    [ForeignKey("TransactionId")]
    public Transaction? Transaction { get; set; }
}