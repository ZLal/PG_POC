namespace PaymentGatewayPOC.Extensions;

public static class IListKeyValuePairExtension
{
    public static string? GetValueByKey(this IList<KeyValuePair<string, string>> data, string key)
    {
        return data.SingleOrDefault(x => string.Equals(x.Key, key, StringComparison.InvariantCultureIgnoreCase)).Value;
    }
}
