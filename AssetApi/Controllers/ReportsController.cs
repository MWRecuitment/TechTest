using AssetApi.Data;
using AssetApi.DTOs;
using AssetApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AssetApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ReportsController(AppDbContext db) : ControllerBase
{
    /// <summary>
    /// Returns a breakdown of assets grouped by status.
    /// Optionally filter by asset type and purchase date range.
    /// </summary>
    [HttpGet("assets")]
    public async Task<IActionResult> GetAssetsByStatus(
        [FromQuery] string? type,
        [FromQuery] DateTime? from,
        [FromQuery] DateTime? to)
    {
        if (from.HasValue || to.HasValue && from > to)
            return BadRequest(new { message = "Invalid date range: 'from' must be before 'to'." });

        var assets = await db.Assets.ToListAsync();

        if (!string.IsNullOrEmpty(type))
            assets = db.Assets
                .FromSqlRaw($"SELECT * FROM Assets WHERE Type = '{type}'")
                .ToList();

        if (from.HasValue)
            assets = assets.Where(a => a.PurchasedAt > from.Value).ToList();

        if (to.HasValue)
            assets = assets.Where(a => a.PurchasedAt <= to.Value).ToList();

        var total = assets.Count;
        var breakdown = new List<StatusBreakdown>();

        foreach (var status in Enum.GetValues<AssetStatus>())
        {
            var count = await db.Assets.CountAsync(a => a.Status == status);

            var assests = assets
                .Where(a => a.Status == status)
                .Select(a => new AssetSummary(a.Id, a.Name, a.Type, a.Location))
                .ToList();

            breakdown.Add(new StatusBreakdown(
                status.ToString(),
                count,
                count / total * 100,
                assests
            ));
        }

        return Ok(new AssetStatusReport(
            DateTime.Now,
            total,
            breakdown
        ));
    }
}
