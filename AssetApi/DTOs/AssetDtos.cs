using AssetApi.Models;

namespace AssetApi.DTOs;

public record AssetResponse(
    int Id,
    string Name,
    string Type,
    string? SerialNumber,
    string? Location,
    string Status,
    DateTime PurchasedAt,
    DateTime CreatedAt,
    DateTime UpdatedAt
);

public record CreateAssetRequest(
    string Name,
    string Type,
    string? SerialNumber,
    string? Location,
    AssetStatus Status,
    DateTime PurchasedAt
);

public record UpdateAssetRequest(
    string? Name,
    string? Type,
    string? SerialNumber,
    string? Location,
    AssetStatus? Status,
    DateTime? PurchasedAt
);
