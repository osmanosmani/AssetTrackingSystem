namespace AssetTrackingSystem.Helpers;

public static class CurrencyHelper
{
    private static readonly Dictionary<string, decimal> UsdExchangeRates = new()
    {
        ["USD"] = 1m,
        ["SEK"] = 10.5m,
        ["EUR"] = 0.92m,
        ["TRY"] = 32m
    };

    public static decimal ConvertFromUsd(decimal usdPrice, string currency)
    {
        if (!UsdExchangeRates.TryGetValue(currency, out decimal rate))
        {
            rate = 1m;
        }

        return Math.Round(usdPrice * rate, 2);
    }
}
