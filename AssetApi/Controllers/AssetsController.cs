using AssetApi.Data;
using AssetApi.DTOs;
using AssetApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AssetApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public class AssetsController(AppDbContext db) : ControllerBase
{
    /// <summary>
    /// Returns all assets. Supports optional filtering by status and type.
    /// </summary>
    [HttpGet]
    [ProducesResponseType<IEnumerable<AssetResponse>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromQuery] AssetStatus? status,
        [FromQuery] string? type)
    {
        var query = db.Assets.AsQueryable();

        if (status.HasValue)
            query = query.Where(a => a.Status == status.Value);

        if (!string.IsNullOrWhiteSpace(type))
            query = query.Where(a => a.Type == type);

        var assets = await query
            .OrderBy(a => a.Name)
            .Select(a => MapToResponse(a))
            .ToListAsync();

        return Ok(assets);
    }

    /// <summary>
    /// Returns a single asset by ID.
    /// </summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType<AssetResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var asset = await db.Assets.FindAsync(id);
        return asset is null ? NotFound() : Ok(MapToResponse(asset));
    }

    /// <summary>
    /// Creates a new asset. Requires Admin role.
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType<AssetResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateAssetRequest request)
    {
        var asset = new Asset
        {
            Name = request.Name,
            Type = request.Type,
            SerialNumber = request.SerialNumber,
            Location = request.Location,
            Status = request.Status,
            PurchasedAt = request.PurchasedAt
        };

        db.Assets.Add(asset);
        await db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = asset.Id }, MapToResponse(asset));
    }

    /// <summary>
    /// Updates an existing asset. Requires Admin role.
    /// </summary>
    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType<AssetResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateAssetRequest request)
    {
        var asset = await db.Assets.FindAsync(id);
        if (asset is null) return NotFound();

        if (request.Name is not null) asset.Name = request.Name;
        if (request.Type is not null) asset.Type = request.Type;
        if (request.SerialNumber is not null) asset.SerialNumber = request.SerialNumber;
        if (request.Location is not null) asset.Location = request.Location;
        if (request.Status.HasValue) asset.Status = request.Status.Value;
        if (request.PurchasedAt.HasValue) asset.PurchasedAt = request.PurchasedAt.Value;
        asset.UpdatedAt = DateTime.UtcNow;

        await db.SaveChangesAsync();
        return Ok(MapToResponse(asset));
    }

    /// <summary>
    /// Deletes an asset by ID. Requires Admin role.
    /// </summary>
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var asset = await db.Assets.FindAsync(id);
        if (asset is null) return NotFound();

        db.Assets.Remove(asset);
        await db.SaveChangesAsync();
        return NoContent();
    }

    private static AssetResponse MapToResponse(Asset a) =>
        new(a.Id, a.Name, a.Type, a.SerialNumber, a.Location,
            a.Status.ToString(), a.PurchasedAt, a.CreatedAt, a.UpdatedAt);
}
