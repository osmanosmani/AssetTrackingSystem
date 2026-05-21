using Microsoft.EntityFrameworkCore;
using AssetTrackingSystem.Data;
using AssetTrackingSystem.Helpers;
using AssetTrackingSystem.Models;

namespace AssetTrackingSystem.Services;

public class AssetService
{
    public List<Asset> GetAllAssets()
    {
        using AppDbContext dbContext = new();

        return dbContext.Assets
            .Include(asset => asset.Office)
            .OrderBy(asset => asset is ComputerAsset ? "Computer" : "Mobile")
            .ThenBy(asset => asset.PurchaseDate)
            .ToList();
    }

    public Asset? GetAssetById(int id)
    {
        using AppDbContext dbContext = new();

        return dbContext.Assets
            .Include(asset => asset.Office)
            .FirstOrDefault(asset => asset.Id == id);
    }

    public void AddAsset()
    {
        using AppDbContext dbContext = new();

        Console.WriteLine("1. Computer asset");
        Console.WriteLine("2. Mobile asset");
        int category = ConsoleHelper.ReadInt("Choose category: ", 1, 2);

        List<Office> offices = dbContext.Offices.OrderBy(office => office.Name).ToList();
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
        else
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

        dbContext.Assets.Add(asset);
        dbContext.SaveChanges();
        Console.WriteLine("Asset added successfully.");
    }

    public void UpdateAsset()
    {
        using AppDbContext dbContext = new();

        int id = ConsoleHelper.ReadInt("Asset id to update: ", 1, int.MaxValue);
        Asset? asset = dbContext.Assets.Include(item => item.Office).FirstOrDefault(item => item.Id == id);

        if (asset is null)
        {
            Console.WriteLine("Asset not found.");
            return;
        }

        List<Office> offices = dbContext.Offices.OrderBy(office => office.Name).ToList();
        Office office = ChooseOffice(offices);

        asset.Brand = ConsoleHelper.ReadRequiredString($"Brand ({asset.Brand}): ");
        asset.ModelName = ConsoleHelper.ReadRequiredString($"Model name ({asset.ModelName}): ");
        asset.PurchaseDate = ConsoleHelper.ReadDate($"Purchase date ({asset.PurchaseDate:yyyy-MM-dd}): ");
        asset.PurchasePriceUsd = ConsoleHelper.ReadPositiveDecimal($"Purchase price USD ({asset.PurchasePriceUsd:N2}): ");
        asset.LocalPrice = CurrencyHelper.ConvertFromUsd(asset.PurchasePriceUsd, office.Currency);
        asset.SerialNumber = ConsoleHelper.ReadRequiredString($"Serial number ({asset.SerialNumber}): ");
        asset.EmployeeUsername = ConsoleHelper.ReadOptionalString($"Employee username ({asset.EmployeeUsername ?? "none"}): ");
        asset.WarrantyExpirationDate = ConsoleHelper.ReadDate($"Warranty expiration ({asset.WarrantyExpirationDate:yyyy-MM-dd}): ");
        asset.OfficeId = office.Id;

        if (asset is ComputerAsset computerAsset)
        {
            computerAsset.ComputerType = ConsoleHelper.ReadRequiredString($"Computer type ({computerAsset.ComputerType}): ");
            computerAsset.AssetType = computerAsset.ComputerType;
            computerAsset.Processor = ConsoleHelper.ReadOptionalString($"Processor ({computerAsset.Processor ?? "none"}): ");
            computerAsset.RamGb = ConsoleHelper.ReadOptionalInt($"RAM GB ({computerAsset.RamGb?.ToString() ?? "none"}): ");
            computerAsset.StorageGb = ConsoleHelper.ReadOptionalInt($"Storage GB ({computerAsset.StorageGb?.ToString() ?? "none"}): ");
        }

        if (asset is MobileAsset mobileAsset)
        {
            mobileAsset.MobileType = ConsoleHelper.ReadRequiredString($"Mobile type ({mobileAsset.MobileType}): ");
            mobileAsset.AssetType = mobileAsset.MobileType;
            mobileAsset.ImeiNumber = ConsoleHelper.ReadOptionalString($"IMEI number ({mobileAsset.ImeiNumber ?? "none"}): ");
            mobileAsset.PhoneNumber = ConsoleHelper.ReadOptionalString($"Phone number ({mobileAsset.PhoneNumber ?? "none"}): ");
        }

        dbContext.SaveChanges();
        Console.WriteLine("Asset updated successfully.");
    }

    public void DeleteAsset()
    {
        using AppDbContext dbContext = new();

        int id = ConsoleHelper.ReadInt("Asset id to delete: ", 1, int.MaxValue);
        Asset? asset = dbContext.Assets.FirstOrDefault(item => item.Id == id);

        if (asset is null)
        {
            Console.WriteLine("Asset not found.");
            return;
        }

        dbContext.Assets.Remove(asset);
        dbContext.SaveChanges();
        Console.WriteLine("Asset deleted successfully.");
    }

    public List<Asset> SearchAssets(string searchText)
    {
        using AppDbContext dbContext = new();
        string value = searchText.ToLower();

        return dbContext.Assets
            .Include(asset => asset.Office)
            .Where(asset =>
                asset.Brand.ToLower().Contains(value) ||
                asset.ModelName.ToLower().Contains(value) ||
                asset.SerialNumber.ToLower().Contains(value) ||
                asset.Office != null && asset.Office.Name.ToLower().Contains(value))
            .OrderBy(asset => asset.Brand)
            .ThenBy(asset => asset.ModelName)
            .ToList();
    }

    public List<Asset> SearchByBrand(string brand)
    {
        using AppDbContext dbContext = new();
        return dbContext.Assets.Include(asset => asset.Office)
            .Where(asset => asset.Brand.Contains(brand))
            .ToList();
    }

    public List<Asset> SearchByModel(string model)
    {
        using AppDbContext dbContext = new();
        return dbContext.Assets.Include(asset => asset.Office)
            .Where(asset => asset.ModelName.Contains(model))
            .ToList();
    }

    public List<Asset> SearchByOffice(string officeName)
    {
        using AppDbContext dbContext = new();
        return dbContext.Assets.Include(asset => asset.Office)
            .Where(asset => asset.Office != null && asset.Office.Name.Contains(officeName))
            .ToList();
    }

    public List<Asset> SearchByPurchaseYear(int year)
    {
        using AppDbContext dbContext = new();
        return dbContext.Assets.Include(asset => asset.Office)
            .Where(asset => asset.PurchaseDate.Year == year)
            .ToList();
    }

    public List<Asset> GetExpiredAssets()
    {
        return GetAllAssets()
            .Where(asset => AssetStatusHelper.GetStatus(asset) == "EXPIRED")
            .ToList();
    }

    public List<Asset> GetComputerAssets()
    {
        using AppDbContext dbContext = new();
        return dbContext.ComputerAssets.Include(asset => asset.Office).Cast<Asset>().ToList();
    }

    public List<Asset> GetMobileAssets()
    {
        using AppDbContext dbContext = new();
        return dbContext.MobileAssets.Include(asset => asset.Office).Cast<Asset>().ToList();
    }

    public List<Asset> GetAssetsByOffice()
    {
        using AppDbContext dbContext = new();
        List<Office> offices = dbContext.Offices.OrderBy(office => office.Name).ToList();
        Office office = ChooseOffice(offices);

        return dbContext.Assets
            .Include(asset => asset.Office)
            .Where(asset => asset.OfficeId == office.Id)
            .ToList();
    }

    private static Office ChooseOffice(List<Office> offices)
    {
        for (int i = 0; i < offices.Count; i++)
        {
            Office office = offices[i];
            Console.WriteLine($"{i + 1}. {office.Name} ({office.Country}, {office.Currency})");
        }

        int choice = ConsoleHelper.ReadInt("Choose office: ", 1, offices.Count);
        return offices[choice - 1];
    }
}
