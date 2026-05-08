using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaymentGatewayPOC.Models;

public class ErrorLog
{
    [Key]
    public Guid LogId { get; set; }

    public Guid? TransactionId { get; set; }

    [Required]
    public string ErrorMessage { get; set; } = string.Empty;

    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    // Navigation properties
    [ForeignKey("TransactionId")]
    public Transaction? Transaction { get; set; }
}