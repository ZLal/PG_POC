namespace PaymentGatewayPOC.Models;

public class AppSettings
{
    public GatewaysSettings Gateways { get; set; } = new();
}

public class GatewaysSettings
{
    public bool Enabled { get; set; } = true;
    public string DisabledMessage { get; set; } = string.Empty;
}
