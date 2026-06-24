namespace PaymentGatewayPOC.Models;

public class GatewayDetail
{
    public Guid GatewayDetailId { get; set; }
    public Guid GatewayId { get; set; }

    public string Key { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;

    // Navigation property
    public Gateway? Gateway { get; set; } = null;
}
