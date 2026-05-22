using AssetTrackingSystem.Helpers;
using AssetTrackingSystem.Models;
using AssetTrackingSystem.Reports;
using AssetTrackingSystem.Services;

AssetService assetService = new();
ReportService reportService = new(assetService);
ExportService exportService = new();

// The main loop keeps Program.cs focused on navigation and delegates business logic to services.
while (true)
{
    ClearConsole();
    Console.WriteLine("Smart Asset Tracking System");
    Console.WriteLine("---------------------------");
    Console.WriteLine("1. Add Asset");
    Console.WriteLine("2. Show All Assets");
    Console.WriteLine("3. Update Asset");
    Console.WriteLine("4. Delete Asset");
    Console.WriteLine("5. Search Asset");
    Console.WriteLine("6. Reports");
    Console.WriteLine("7. Filters");
    Console.WriteLine("8. Export");
    Console.WriteLine("9. Exit");
    Console.WriteLine();

    int choice = ConsoleHelper.ReadInt("Choose option: ", 1, 9);
    Console.WriteLine();

    try
    {
        switch (choice)
        {
            case 1:
                await assetService.AddAssetAsync();
                break;
            case 2:
                reportService.PrintAssets(await assetService.GetAllAssetsAsync(), "ASSET LIST");
                break;
            case 3:
                await assetService.UpdateAssetAsync();
                break;
            case 4:
                await assetService.DeleteAssetAsync();
                break;
            case 5:
                await ShowSearchMenuAsync();
                break;
            case 6:
                await ShowReportsMenuAsync();
                break;
            case 7:
                await ShowFiltersMenuAsync();
                break;
            case 8:
                await ShowExportMenuAsync();
                break;
            case 9:
                return;
        }
    }
    catch (Exception exception)
    {
        Console.WriteLine($"Something went wrong: {exception.Message}");
    }

    ConsoleHelper.Pause();
}

void ClearConsole()
{
    try
    {
        Console.Clear();
    }
    catch (IOException)
    {
        Console.WriteLine();
    }
}

async Task ShowSearchMenuAsync()
{
    Console.WriteLine("1. General search");
    Console.WriteLine("2. Search by brand");
    Console.WriteLine("3. Search by model");
    Console.WriteLine("4. Search by office");
    Console.WriteLine("5. Search by purchase year");

    int choice = ConsoleHelper.ReadInt("Choose search option: ", 1, 5);
    List<Asset> results;

    switch (choice)
    {
        case 1:
            string searchText = ConsoleHelper.ReadRequiredString("Search text: ");
            results = await assetService.SearchAssetsAsync(searchText);
            PrintSearchResults(results, $"Search: Text = {searchText}");
            break;
        case 2:
            string brand = ConsoleHelper.ReadRequiredString("Brand: ");
            results = await assetService.SearchByBrandAsync(brand);
            PrintSearchResults(results, $"Search: Brand = {brand}");
            break;
        case 3:
            string model = ConsoleHelper.ReadRequiredString("Model: ");
            results = await assetService.SearchByModelAsync(model);
            PrintSearchResults(results, $"Search: Model = {model}");
            break;
        case 4:
            string office = ConsoleHelper.ReadRequiredString("Office: ");
            results = await assetService.SearchByOfficeAsync(office);
            PrintSearchResults(results, $"Search: Office = {office}");
            break;
        default:
            int year = ConsoleHelper.ReadInt("Purchase year: ", 1900, DateTime.Today.Year + 1);
            results = await assetService.SearchByPurchaseYearAsync(year);
            PrintSearchResults(results, $"Search: Purchase Year = {year}");
            break;
    }
}

void PrintSearchResults(List<Asset> results, string subtitle)
{
    Console.WriteLine();
    reportService.PrintAssets(results, "SEARCH RESULT", subtitle);
}

async Task ShowReportsMenuAsync()
{
    Console.WriteLine("1. Total asset value per office");
    Console.WriteLine("2. Asset count per office");
    Console.WriteLine("3. Assets close to expiration");
    Console.WriteLine("4. Most expensive assets");

    int choice = ConsoleHelper.ReadInt("Choose report: ", 1, 4);
    Console.WriteLine();

    switch (choice)
    {
        case 1:
            await reportService.ShowTotalValuePerOfficeAsync();
            break;
        case 2:
            await reportService.ShowAssetCountPerOfficeAsync();
            break;
        case 3:
            await reportService.ShowAssetsCloseToExpirationAsync();
            break;
        case 4:
            await reportService.ShowMostExpensiveAssetsAsync();
            break;
    }
}

async Task ShowFiltersMenuAsync()
{
    Console.WriteLine("1. Expired assets");
    Console.WriteLine("2. Only computers");
    Console.WriteLine("3. Only mobile devices");
    Console.WriteLine("4. Assets by office");

    int choice = ConsoleHelper.ReadInt("Choose filter: ", 1, 4);
    Console.WriteLine();

    List<Asset> assets = choice switch
    {
        1 => await assetService.GetExpiredAssetsAsync(),
        2 => await assetService.GetComputerAssetsAsync(),
        3 => await assetService.GetMobileAssetsAsync(),
        _ => await assetService.GetAssetsByOfficeAsync()
    };

    string title = choice switch
    {
        1 => "EXPIRED ASSETS",
        2 => "COMPUTER ASSETS",
        3 => "MOBILE ASSETS",
        _ => "OFFICE ASSETS"
    };

    reportService.PrintAssets(assets, title);
}

async Task ShowExportMenuAsync()
{
    List<Asset> assets = await assetService.GetAllAssetsAsync();

    Console.WriteLine("1. Export TXT");
    Console.WriteLine("2. Export CSV");
    Console.WriteLine("3. Export JSON");
    Console.WriteLine("4. Export all");

    int choice = ConsoleHelper.ReadInt("Choose export option: ", 1, 4);

    switch (choice)
    {
        case 1:
            exportService.ExportToTxt(assets);
            break;
        case 2:
            exportService.ExportToCsv(assets);
            break;
        case 3:
            exportService.ExportToJson(assets);
            break;
        case 4:
            exportService.ExportToTxt(assets);
            exportService.ExportToCsv(assets);
            exportService.ExportToJson(assets);
            break;
    }
}
