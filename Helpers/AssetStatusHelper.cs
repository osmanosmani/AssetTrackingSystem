using AssetTrackingSystem.Models;

namespace AssetTrackingSystem.Helpers;

public static class AssetStatusHelper
{
    public static string GetStatus(Asset asset)
    {
        DateTime endOfLifeDate = asset.PurchaseDate.AddYears(3);
        DateTime today = DateTime.Today;

        // RED is treated as the most urgent warning even though the PDF wording is inconsistent.
        if (endOfLifeDate < today)
        {
            return "EXPIRED";
        }

        if (endOfLifeDate < today.AddMonths(3))
        {
            return "RED";
        }

        if (endOfLifeDate < today.AddMonths(6))
        {
            return "YELLOW";
        }

        return "NORMAL";
    }

    public static void SetStatusColor(string status)
    {
        Console.ForegroundColor = status switch
        {
            "EXPIRED" => ConsoleColor.DarkRed,
            "RED" => ConsoleColor.Red,
            "YELLOW" => ConsoleColor.Yellow,
            _ => ConsoleColor.Gray
        };
    }
}
