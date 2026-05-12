namespace AssetApi.Models;

public class Asset
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Type { get; set; }
    public string? SerialNumber { get; set; }
    public string? Location { get; set; }
    public AssetStatus Status { get; set; } = AssetStatus.Active;
    public DateTime PurchasedAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}

public enum AssetStatus
{
    Active,
    InMaintenance,
    Retired
}
