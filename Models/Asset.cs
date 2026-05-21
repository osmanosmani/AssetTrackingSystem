namespace AssetTrackingSystem.Models;

public class Asset
{
    public Asset()
    {
    }

    public Asset(
        string assetType,
        string brand,
        string modelName,
        DateTime purchaseDate,
        decimal purchasePriceUsd,
        decimal localPrice,
        string serialNumber,
        DateTime warrantyExpirationDate,
        int officeId,
        string? employeeUsername = null)
    {
        AssetType = assetType;
        Brand = brand;
        ModelName = modelName;
        PurchaseDate = purchaseDate;
        PurchasePriceUsd = purchasePriceUsd;
        LocalPrice = localPrice;
        SerialNumber = serialNumber;
        WarrantyExpirationDate = warrantyExpirationDate;
        OfficeId = officeId;
        EmployeeUsername = employeeUsername;
    }

    public int Id { get; set; }

    public string AssetType { get; set; } = string.Empty;

    public string Brand { get; set; } = string.Empty;

    public string ModelName { get; set; } = string.Empty;

    public DateTime PurchaseDate { get; set; }

    public decimal PurchasePriceUsd { get; set; }

    public decimal LocalPrice { get; set; }

    public string SerialNumber { get; set; } = string.Empty;

    public string? EmployeeUsername { get; set; }

    public DateTime WarrantyExpirationDate { get; set; }

    public int OfficeId { get; set; }

    public Office? Office { get; set; }
}
