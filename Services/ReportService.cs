using AssetTrackingSystem.Helpers;
using AssetTrackingSystem.Models;

namespace AssetTrackingSystem.Services;

public class ReportService
{
    private const int PageSize = 5;
    private readonly AssetService assetService;

    public ReportService(AssetService assetService)
    {
        this.assetService = assetService;
    }

    public async Task ShowTotalValuePerOfficeAsync()
    {
        var report = (await assetService.GetAllAssetsAsync())
            .Where(asset => asset.Office != null)
            .GroupBy(asset => asset.Office!)
            .Select(group => new
            {
                Office = group.Key,
                TotalUsd = group.Sum(asset => asset.PurchasePriceUsd),
                TotalLocal = group.Sum(asset => asset.LocalPrice)
            })
            .OrderBy(item => item.Office.Name);

        PrintTitle("OFFICE VALUE REPORT");
        Console.WriteLine($"{"Office",-20} {"Total USD",12} {"Local Value",16}");
        Console.WriteLine(new string('-', 52));

        foreach (var item in report)
        {
            Console.WriteLine($"{item.Office.Name,-20} {item.TotalUsd,9:N2} USD {item.TotalLocal,10:N2} {item.Office.Currency}");
        }

        PrintFooter();
    }

    public async Task ShowAssetCountPerOfficeAsync()
    {
        List<Asset> allAssets = await assetService.GetAllAssetsAsync();

        var report = allAssets
            .Where(asset => asset.Office != null)
            .GroupBy(asset => asset.Office!.Name)
            .Select(group => new { Office = group.Key, Count = group.Count() })
            .OrderBy(item => item.Office);

        PrintTitle("REPORT");
        Console.WriteLine("Office Asset Counts");
        Console.WriteLine(new string('-', 40));

        foreach (var item in report)
        {
            Console.WriteLine($"{item.Office,-18}: {item.Count}");
        }

        Console.WriteLine();
        Console.WriteLine("Assets Near Expiration");
        Console.WriteLine(new string('-', 40));

        List<Asset> nearExpirationAssets = allAssets
            .Where(IsNearExpiration)
            .OrderBy(asset => asset.PurchaseDate.AddYears(3))
            .ToList();

        if (nearExpirationAssets.Count == 0)
        {
            Console.WriteLine("No assets near expiration.");
        }
        else
        {
            foreach (Asset asset in nearExpirationAssets)
            {
                Console.WriteLine($"{asset.Brand} {asset.ModelName}");
            }
        }

        PrintFooter();
    }

    public async Task ShowAssetsCloseToExpirationAsync()
    {
        List<Asset> assets = (await assetService.GetAllAssetsAsync())
            .Where(IsNearExpiration)
            .OrderBy(asset => asset.PurchaseDate.AddYears(3))
            .ToList();

        PrintAssets(assets, "ASSETS NEAR EXPIRATION");
    }

    public async Task ShowMostExpensiveAssetsAsync()
    {
        List<Asset> assets = (await assetService.GetAllAssetsAsync())
            .OrderByDescending(asset => asset.PurchasePriceUsd)
            .Take(5)
            .ToList();

        PrintAssets(assets, "MOST EXPENSIVE ASSETS");
    }

    public void PrintAssets(List<Asset> assets, string title = "ASSET LIST", string? subtitle = null)
    {
        PrintTitle(title);

        if (!string.IsNullOrWhiteSpace(subtitle))
        {
            Console.WriteLine(subtitle);
            Console.WriteLine();
        }

        if (assets.Count == 0)
        {
            Console.WriteLine("No assets found.");
            PrintFooter();
            return;
        }

        if (assets.Count <= PageSize)
        {
            PrintAssetTable(assets);
        }
        else
        {
            PrintAssetsWithPagination(assets);
        }

        PrintTotalWhenSingleCurrency(assets);
        PrintFooter();
    }

    public static string GetCategoryName(Asset asset)
    {
        return asset switch
        {
            ComputerAsset => "Computer",
            MobileAsset => "Mobile",
            _ => "General"
        };
    }

    private static bool IsNearExpiration(Asset asset)
    {
        string status = AssetStatusHelper.GetStatus(asset);
        return status is "RED" or "YELLOW" or "EXPIRED";
    }

    private static void PrintAssetsWithPagination(List<Asset> assets)
    {
        int page = 0;
        int totalPages = (int)Math.Ceiling(assets.Count / (double)PageSize);

        while (true)
        {
            List<Asset> currentPageAssets = assets.Skip(page * PageSize).Take(PageSize).ToList();
            PrintAssetTable(currentPageAssets);
            Console.WriteLine($"Page {page + 1} of {totalPages}");

            if (totalPages == 1)
            {
                return;
            }

            Console.Write("N = next, P = previous, Q = quit: ");
            string? input = Console.ReadLine()?.Trim().ToUpper();

            if (input == "N" && page < totalPages - 1)
            {
                page++;
            }
            else if (input == "P" && page > 0)
            {
                page--;
            }
            else if (input == "Q" || string.IsNullOrWhiteSpace(input))
            {
                return;
            }

            Console.WriteLine();
        }
    }

    private static void PrintAssetTable(List<Asset> assets)
    {
        Console.WriteLine($"{"ID",-4} {"Type",-16} {"Brand",-14} {"Model",-20} {"Office",-16} {"Price",-16} {"Status",-8}");
        Console.WriteLine(new string('-', 104));

        foreach (Asset asset in assets)
        {
            string status = AssetStatusHelper.GetStatus(asset);
            string currency = asset.Office?.Currency ?? "";
            string price = $"{asset.LocalPrice:N0} {currency}".Trim();

            Console.Write($"{asset.Id,-4} {asset.AssetType,-16} {asset.Brand,-14} {asset.ModelName,-20} {asset.Office?.Name,-16} {price,-16} ");
            AssetStatusHelper.SetStatusColor(status);
            Console.WriteLine($"{status,-8}");
            Console.ResetColor();
        }
    }

    private static void PrintTotalWhenSingleCurrency(List<Asset> assets)
    {
        decimal totalValue = assets.Sum(asset => asset.LocalPrice);
        List<string?> currencies = assets
            .Select(asset => asset.Office?.Currency)
            .Where(currency => !string.IsNullOrWhiteSpace(currency))
            .Distinct()
            .ToList();

        if (currencies.Count == 1)
        {
            Console.WriteLine();
            Console.WriteLine($"Total Value: {totalValue:N0} {currencies[0]}");
        }
    }

    private static void PrintTitle(string title)
    {
        Console.WriteLine($"================ {title} ================");
        Console.WriteLine();
    }

    private static void PrintFooter()
    {
        Console.WriteLine();
        Console.WriteLine("====================================================");
    }
}
