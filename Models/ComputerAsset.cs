namespace AssetTrackingSystem.Models;

public class ComputerAsset : Asset
{
    public ComputerAsset()
    {
    }

    public ComputerAsset(
        string computerType,
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
            computerType,
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
        ComputerType = computerType;
    }

    public string ComputerType { get; set; } = string.Empty;

    public string? Processor { get; set; }

    public int? RamGb { get; set; }

    public int? StorageGb { get; set; }
}
