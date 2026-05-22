using System.Text;
using System.Text.Json;
using AssetTrackingSystem.Helpers;
using AssetTrackingSystem.Models;
using AssetTrackingSystem.Services;

namespace AssetTrackingSystem.Reports;

public class ExportService
{
    private const string ExportFolder = "Reports";

    public void ExportToTxt(List<Asset> assets)
    {
        EnsureExportFolderExists();
        string path = Path.Combine(ExportFolder, "assets-report.txt");

        // TXT export mirrors the console table so it is easy to read without spreadsheet software.
        StringBuilder builder = new();

        builder.AppendLine("================ ASSET EXPORT ================");
        builder.AppendLine();
        builder.AppendLine($"{"ID",-4} {"Type",-16} {"Brand",-14} {"Model",-20} {"Office",-16} {"Status",-8}");
        builder.AppendLine(new string('-', 82));

        foreach (Asset asset in assets)
        {
            builder.AppendLine($"{asset.Id,-4} {asset.AssetType,-16} {asset.Brand,-14} {asset.ModelName,-20} {asset.Office?.Name,-16} {AssetStatusHelper.GetStatus(asset),-8}");
        }

        builder.AppendLine();
        builder.AppendLine("====================================================");

        File.WriteAllText(path, builder.ToString());
        Console.WriteLine($"TXT export created: {Path.GetFullPath(path)}");
    }

    public void ExportToCsv(List<Asset> assets)
    {
        EnsureExportFolderExists();
        string path = Path.Combine(ExportFolder, "assets-report.csv");

        // CSV is intentionally flat so it can be opened directly in Excel or similar tools.
        StringBuilder builder = new();
        builder.AppendLine("Id,Type,Brand,Model,Office,Price,Status");

        foreach (Asset asset in assets)
        {
            string line = string.Join(",",
                asset.Id,
                Escape(asset.AssetType),
                Escape(asset.Brand),
                Escape(asset.ModelName),
                Escape(asset.Office?.Name ?? ""),
                Escape($"{asset.LocalPrice:0.00} {asset.Office?.Currency}"),
                Escape(AssetStatusHelper.GetStatus(asset)));

            builder.AppendLine(line);
        }

        File.WriteAllText(path, builder.ToString());
        Console.WriteLine($"CSV export created: {Path.GetFullPath(path)}");
    }

    public void ExportToJson(List<Asset> assets)
    {
        EnsureExportFolderExists();
        string path = Path.Combine(ExportFolder, "assets-report.json");

        // JSON keeps richer field names for systems that may consume the report programmatically.
        var exportData = assets.Select(asset => new
        {
            asset.Id,
            Category = ReportService.GetCategoryName(asset),
            asset.AssetType,
            asset.Brand,
            asset.ModelName,
            Office = asset.Office?.Name,
            asset.PurchaseDate,
            asset.PurchasePriceUsd,
            asset.LocalPrice,
            Currency = asset.Office?.Currency,
            asset.SerialNumber,
            asset.EmployeeUsername,
            Status = AssetStatusHelper.GetStatus(asset)
        });

        JsonSerializerOptions options = new() { WriteIndented = true };
        File.WriteAllText(path, JsonSerializer.Serialize(exportData, options));
        Console.WriteLine($"JSON export created: {Path.GetFullPath(path)}");
    }

    private static void EnsureExportFolderExists()
    {
        Directory.CreateDirectory(ExportFolder);
    }

    private static string Escape(string value)
    {
        return $"\"{value.Replace("\"", "\"\"")}\"";
    }
}
