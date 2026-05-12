namespace AssetApi.DTOs;

public record AssetStatusReport(
    DateTime GeneratedAt,
    int TotalAssets,
    IReadOnlyList<StatusBreakdown> Breakdown
);

public record StatusBreakdown(
    string Status,
    int Count,
    int Percentage,
    IReadOnlyList<AssetSummary> Assets
);

public record AssetSummary(
    int Id,
    string Name,
    string Type,
    string? Location
);
