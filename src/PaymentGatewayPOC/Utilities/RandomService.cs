using PaymentGatewayPOC.Utilities.Interfaces;

namespace PaymentGatewayPOC.Utilities;

public class RandomService : IRandomService
{
    public string GenerateRandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        return new string([.. Enumerable.Repeat(chars, length).Select(s => s[Random.Shared.Next(s.Length)])]);
    }
}
