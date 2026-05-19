using System.ComponentModel.DataAnnotations.Schema;

namespace PaymentGatewayPOC.Models;

public class ApplicationGateway
{
    public Guid ApplicationId { get; set; }

    public Guid GatewayId { get; set; }

    [ForeignKey("ApplicationId")]
    public Application? Application { get; set; }

    [ForeignKey("GatewayId")]
    public Gateway? Gateway { get; set; }
}
