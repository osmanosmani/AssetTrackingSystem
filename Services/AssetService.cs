using Microsoft.EntityFrameworkCore;
using AssetTrackingSystem.Data;
using AssetTrackingSystem.Helpers;
using AssetTrackingSystem.Models;

namespace AssetTrackingSystem.Services;

public class AssetService
{
    public async Task<List<Asset>> GetAllAssetsAsync()
    {
        using AppDbContext dbContext = new();

        // EF Core loads the derived TPT rows automatically when querying the base Assets set.
        return await dbContext.Assets
            .Include(asset => asset.Office)
            .OrderBy(asset => asset is ComputerAsset ? "Computer" : asset is MobileAsset ? "Mobile" : "General")
            .ThenBy(asset => asset.PurchaseDate)
            .ToListAsync();
    }

    public async Task AddAssetAsync()
    {
        using AppDbContext dbContext = new();

        Console.WriteLine("1. Computer asset");
        Console.WriteLine("2. Mobile asset");
        Console.WriteLine("3. Office equipment");
        int category = ConsoleHelper.ReadInt("Choose category: ", 1, 3);

        List<Office> offices = await dbContext.Offices.OrderBy(office => office.Name).ToListAsync();
        Office office = ChooseOffice(offices);

        string brand = ConsoleHelper.ReadRequiredString("Brand: ");
        string modelName = ConsoleHelper.ReadRequiredString("Model name: ");
        DateTime purchaseDate = ConsoleHelper.ReadDate("Purchase date (yyyy-mm-dd): ");
        decimal purchasePriceUsd = ConsoleHelper.ReadPositiveDecimal("Purchase price USD: ");
        string serialNumber = ConsoleHelper.ReadRequiredString("Serial number: ");
        string? employeeUsername = ConsoleHelper.ReadOptionalString("Employee username (optional): ");
        DateTime warrantyDate = ConsoleHelper.ReadDate("Warranty expiration date (yyyy-mm-dd): ");
        decimal localPrice = CurrencyHelper.ConvertFromUsd(purchasePriceUsd, office.Currency);

        Asset asset;

        if (category == 1)
        {
            string computerType = ConsoleHelper.ReadRequiredString("Computer type (Laptop/Desktop): ");

            asset = new ComputerAsset
            {
                AssetType = computerType,
                ComputerType = computerType,
                Brand = brand,
                ModelName = modelName,
                PurchaseDate = purchaseDate,
                PurchasePriceUsd = purchasePriceUsd,
                LocalPrice = localPrice,
                SerialNumber = serialNumber,
                EmployeeUsername = employeeUsername,
                WarrantyExpirationDate = warrantyDate,
                OfficeId = office.Id,
                Processor = ConsoleHelper.ReadOptionalString("Processor (optional): "),
                RamGb = ConsoleHelper.ReadOptionalInt("RAM GB (optional): "),
                StorageGb = ConsoleHelper.ReadOptionalInt("Storage GB (optional): ")
            };
        }
        else if (category == 2)
        {
            string mobileType = ConsoleHelper.ReadRequiredString("Mobile type (iPhone/Samsung/Nokia/Tablet): ");

            asset = new MobileAsset
            {
                AssetType = mobileType,
                MobileType = mobileType,
                Brand = brand,
                ModelName = modelName,
                PurchaseDate = purchaseDate,
                PurchasePriceUsd = purchasePriceUsd,
                LocalPrice = localPrice,
                SerialNumber = serialNumber,
                EmployeeUsername = employeeUsername,
                WarrantyExpirationDate = warrantyDate,
                OfficeId = office.Id,
                ImeiNumber = ConsoleHelper.ReadOptionalString("IMEI number (optional): "),
                PhoneNumber = ConsoleHelper.ReadOptionalString("Phone number (optional): ")
            };
        }
        else
        {
            asset = new Asset
            {
                AssetType = "Office Equipment",
                Brand = brand,
                ModelName = modelName,
                PurchaseDate = purchaseDate,
                PurchasePriceUsd = purchasePriceUsd,
                LocalPrice = localPrice,
                SerialNumber = serialNumber,
                EmployeeUsername = employeeUsername,
                WarrantyExpirationDate = warrantyDate,
                OfficeId = office.Id
            };
        }

        dbContext.Assets.Add(asset);
        await dbContext.SaveChangesAsync();
        Console.WriteLine("Asset added successfully.");
    }

    public async Task UpdateAssetAsync()
    {
        using AppDbContext dbContext = new();

        int id = ConsoleHelper.ReadInt("Asset id to update: ", 1, int.MaxValue);
        Asset? asset = await dbContext.Assets.Include(item => item.Office).FirstOrDefaultAsync(item => item.Id == id);

        if (asset is null)
        {
            Console.WriteLine("Asset not found.");
            return;
        }

        Console.WriteLine("Press Enter to keep the current value.");
        Console.WriteLine();

        List<Office> offices = await dbContext.Offices.OrderBy(office => office.Name).ToListAsync();
        Office office = ChooseOffice(offices, asset.OfficeId);

        asset.Brand = ConsoleHelper.ReadRequiredStringOrDefault($"Brand ({asset.Brand}): ", asset.Brand);
        asset.ModelName = ConsoleHelper.ReadRequiredStringOrDefault($"Model name ({asset.ModelName}): ", asset.ModelName);
        asset.PurchaseDate = ConsoleHelper.ReadDateOrDefault($"Purchase date ({asset.PurchaseDate:yyyy-MM-dd}): ", asset.PurchaseDate);
        asset.PurchasePriceUsd = ConsoleHelper.ReadPositiveDecimalOrDefault($"Purchase price USD ({asset.PurchasePriceUsd:N2}): ", asset.PurchasePriceUsd);
        asset.LocalPrice = CurrencyHelper.ConvertFromUsd(asset.PurchasePriceUsd, office.Currency);
        asset.SerialNumber = ConsoleHelper.ReadRequiredStringOrDefault($"Serial number ({asset.SerialNumber}): ", asset.SerialNumber);
        asset.EmployeeUsername = ConsoleHelper.ReadOptionalStringOrDefault($"Employee username ({asset.EmployeeUsername ?? "none"}): ", asset.EmployeeUsername);
        asset.WarrantyExpirationDate = ConsoleHelper.ReadDateOrDefault($"Warranty expiration ({asset.WarrantyExpirationDate:yyyy-MM-dd}): ", asset.WarrantyExpirationDate);
        asset.OfficeId = office.Id;

        if (asset is ComputerAsset computerAsset)
        {
            computerAsset.ComputerType = ConsoleHelper.ReadRequiredStringOrDefault($"Computer type ({computerAsset.ComputerType}): ", computerAsset.ComputerType);
            computerAsset.AssetType = computerAsset.ComputerType;
            computerAsset.Processor = ConsoleHelper.ReadOptionalStringOrDefault($"Processor ({computerAsset.Processor ?? "none"}): ", computerAsset.Processor);
            computerAsset.RamGb = ConsoleHelper.ReadOptionalIntOrDefault($"RAM GB ({computerAsset.RamGb?.ToString() ?? "none"}): ", computerAsset.RamGb);
            computerAsset.StorageGb = ConsoleHelper.ReadOptionalIntOrDefault($"Storage GB ({computerAsset.StorageGb?.ToString() ?? "none"}): ", computerAsset.StorageGb);
        }

        if (asset is MobileAsset mobileAsset)
        {
            mobileAsset.MobileType = ConsoleHelper.ReadRequiredStringOrDefault($"Mobile type ({mobileAsset.MobileType}): ", mobileAsset.MobileType);
            mobileAsset.AssetType = mobileAsset.MobileType;
            mobileAsset.ImeiNumber = ConsoleHelper.ReadOptionalStringOrDefault($"IMEI number ({mobileAsset.ImeiNumber ?? "none"}): ", mobileAsset.ImeiNumber);
            mobileAsset.PhoneNumber = ConsoleHelper.ReadOptionalStringOrDefault($"Phone number ({mobileAsset.PhoneNumber ?? "none"}): ", mobileAsset.PhoneNumber);
        }

        await dbContext.SaveChangesAsync();
        Console.WriteLine("Asset updated successfully.");
    }

    public async Task DeleteAssetAsync()
    {
        using AppDbContext dbContext = new();

        int id = ConsoleHelper.ReadInt("Asset id to delete: ", 1, int.MaxValue);
        Asset? asset = await dbContext.Assets.FirstOrDefaultAsync(item => item.Id == id);

        if (asset is null)
        {
            Console.WriteLine("Asset not found.");
            return;
        }

        dbContext.Assets.Remove(asset);
        await dbContext.SaveChangesAsync();
        Console.WriteLine("Asset deleted successfully.");
    }

    public async Task<List<Asset>> SearchAssetsAsync(string searchText)
    {
        using AppDbContext dbContext = new();
        string value = searchText.ToLower();

        return await dbContext.Assets
            .Include(asset => asset.Office)
            .Where(asset =>
                asset.Brand.ToLower().Contains(value) ||
                asset.ModelName.ToLower().Contains(value) ||
                asset.SerialNumber.ToLower().Contains(value) ||
                asset.Office != null && asset.Office.Name.ToLower().Contains(value))
            .OrderBy(asset => asset.Brand)
            .ThenBy(asset => asset.ModelName)
            .ToListAsync();
    }

    public async Task<List<Asset>> SearchByBrandAsync(string brand)
    {
        using AppDbContext dbContext = new();
        return await dbContext.Assets.Include(asset => asset.Office)
            .Where(asset => asset.Brand.Contains(brand))
            .ToListAsync();
    }

    public async Task<List<Asset>> SearchByModelAsync(string model)
    {
        using AppDbContext dbContext = new();
        return await dbContext.Assets.Include(asset => asset.Office)
            .Where(asset => asset.ModelName.Contains(model))
            .ToListAsync();
    }

    public async Task<List<Asset>> SearchByOfficeAsync(string officeName)
    {
        using AppDbContext dbContext = new();
        return await dbContext.Assets.Include(asset => asset.Office)
            .Where(asset => asset.Office != null && asset.Office.Name.Contains(officeName))
            .ToListAsync();
    }

    public async Task<List<Asset>> SearchByPurchaseYearAsync(int year)
    {
        using AppDbContext dbContext = new();
        return await dbContext.Assets.Include(asset => asset.Office)
            .Where(asset => asset.PurchaseDate.Year == year)
            .ToListAsync();
    }

    public async Task<List<Asset>> GetExpiredAssetsAsync()
    {
        List<Asset> assets = await GetAllAssetsAsync();
        return assets.Where(asset => AssetStatusHelper.GetStatus(asset) == "EXPIRED").ToList();
    }

    public async Task<List<Asset>> GetComputerAssetsAsync()
    {
        using AppDbContext dbContext = new();
        return await dbContext.ComputerAssets.Include(asset => asset.Office).Cast<Asset>().ToListAsync();
    }

    public async Task<List<Asset>> GetMobileAssetsAsync()
    {
        using AppDbContext dbContext = new();
        return await dbContext.MobileAssets.Include(asset => asset.Office).Cast<Asset>().ToListAsync();
    }

    public async Task<List<Asset>> GetAssetsByOfficeAsync()
    {
        using AppDbContext dbContext = new();
        List<Office> offices = await dbContext.Offices.OrderBy(office => office.Name).ToListAsync();
        Office office = ChooseOffice(offices);

        return await dbContext.Assets
            .Include(asset => asset.Office)
            .Where(asset => asset.OfficeId == office.Id)
            .ToListAsync();
    }

    private static Office ChooseOffice(List<Office> offices, int? currentOfficeId = null)
    {
        for (int i = 0; i < offices.Count; i++)
        {
            Office office = offices[i];
            string currentMarker = office.Id == currentOfficeId ? " (current)" : "";
            Console.WriteLine($"{i + 1}. {office.Name} ({office.Country}, {office.Currency}){currentMarker}");
        }

        int choice = currentOfficeId is null
            ? ConsoleHelper.ReadInt("Choose office: ", 1, offices.Count)
            : ConsoleHelper.ReadIntOrDefault("Choose office: ", 1, offices.Count, offices.FindIndex(office => office.Id == currentOfficeId) + 1);

        return offices[choice - 1];
    }
}
