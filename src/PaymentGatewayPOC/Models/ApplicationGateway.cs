namespace PaymentGatewayPOC.Models;

public class ApplicationGateway
{
    public Guid ApplicationId { get; set; }

    public Guid GatewayId { get; set; }

    // Navigation properties
    public Application? Application { get; set; }

    public Gateway? Gateway { get; set; }
}
