using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaymentGatewayPOC.Models;

public class Transaction
{
    [Key]
    public Guid TransactionId { get; set; }

    [Required]
    public Guid ApplicationId { get; set; }

    [Required]
    public Guid GatewayId { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Amount { get; set; }

    [Required]
    public TransactionStatus Status { get; set; } = TransactionStatus.Pending;

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    // Navigation properties
    [ForeignKey("ApplicationId")]
    public Application? Application { get; set; }

    [ForeignKey("GatewayId")]
    public Gateway? Gateway { get; set; }

    public ICollection<TransactionDetail> TransactionDetails { get; set; } = new List<TransactionDetail>();
    public ICollection<ErrorLog> ErrorLogs { get; set; } = new List<ErrorLog>();
}