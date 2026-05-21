namespace AssetTrackingSystem.Models;

public class Office
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Country { get; set; } = string.Empty;

    public string Currency { get; set; } = string.Empty;

    public List<Asset> Assets { get; set; } = new();
}
