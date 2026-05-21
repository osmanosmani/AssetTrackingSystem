using AssetTrackingSystem.Helpers;
using AssetTrackingSystem.Models;
using AssetTrackingSystem.Reports;
using AssetTrackingSystem.Services;

AssetService assetService = new();
ReportService reportService = new(assetService);
ExportService exportService = new();

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
                assetService.AddAsset();
                break;
            case 2:
                reportService.PrintAssets(assetService.GetAllAssets(), "ASSET LIST");
                break;
            case 3:
                assetService.UpdateAsset();
                break;
            case 4:
                assetService.DeleteAsset();
                break;
            case 5:
                ShowSearchMenu();
                break;
            case 6:
                ShowReportsMenu();
                break;
            case 7:
                ShowFiltersMenu();
                break;
            case 8:
                ShowExportMenu();
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

void ShowSearchMenu()
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
            results = assetService.SearchAssets(searchText);
            PrintSearchResults(results, $"Search: Text = {searchText}");
            break;
        case 2:
            string brand = ConsoleHelper.ReadRequiredString("Brand: ");
            results = assetService.SearchByBrand(brand);
            PrintSearchResults(results, $"Search: Brand = {brand}");
            break;
        case 3:
            string model = ConsoleHelper.ReadRequiredString("Model: ");
            results = assetService.SearchByModel(model);
            PrintSearchResults(results, $"Search: Model = {model}");
            break;
        case 4:
            string office = ConsoleHelper.ReadRequiredString("Office: ");
            results = assetService.SearchByOffice(office);
            PrintSearchResults(results, $"Search: Office = {office}");
            break;
        default:
            int year = ConsoleHelper.ReadInt("Purchase year: ", 1900, DateTime.Today.Year + 1);
            results = assetService.SearchByPurchaseYear(year);
            PrintSearchResults(results, $"Search: Purchase Year = {year}");
            break;
    }
}

void PrintSearchResults(List<Asset> results, string subtitle)
{
    Console.WriteLine();
    reportService.PrintAssets(results, "SEARCH RESULT", subtitle);
}

void ShowReportsMenu()
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
            reportService.ShowTotalValuePerOffice();
            break;
        case 2:
            reportService.ShowAssetCountPerOffice();
            break;
        case 3:
            reportService.ShowAssetsCloseToExpiration();
            break;
        case 4:
            reportService.ShowMostExpensiveAssets();
            break;
    }
}

void ShowFiltersMenu()
{
    Console.WriteLine("1. Expired assets");
    Console.WriteLine("2. Only computers");
    Console.WriteLine("3. Only mobile devices");
    Console.WriteLine("4. Assets by office");

    int choice = ConsoleHelper.ReadInt("Choose filter: ", 1, 4);
    Console.WriteLine();

    List<Asset> assets = choice switch
    {
        1 => assetService.GetExpiredAssets(),
        2 => assetService.GetComputerAssets(),
        3 => assetService.GetMobileAssets(),
        _ => assetService.GetAssetsByOffice()
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

void ShowExportMenu()
{
    List<Asset> assets = assetService.GetAllAssets();

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
