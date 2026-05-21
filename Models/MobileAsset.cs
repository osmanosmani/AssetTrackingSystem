namespace AssetTrackingSystem.Models;

public class MobileAsset : Asset
{
    public MobileAsset()
    {
    }

    public MobileAsset(
        string mobileType,
        string brand,
        string modelName,
        DateTime purchaseDate,
        decimal purchasePriceUsd,
        decimal localPrice,
        string serialNumber,
        DateTime warrantyExpirationDate,
        int officeId,
        string? employeeUsername = null)
        : base(
            mobileType,
            brand,
            modelName,
            purchaseDate,
            purchasePriceUsd,
            localPrice,
            serialNumber,
            warrantyExpirationDate,
            officeId,
            employeeUsername)
    {
        MobileType = mobileType;
    }

    public string MobileType { get; set; } = string.Empty;

    public string? ImeiNumber { get; set; }

    public string? PhoneNumber { get; set; }
}
