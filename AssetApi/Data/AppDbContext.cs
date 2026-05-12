using AssetApi.Models;
using Microsoft.EntityFrameworkCore;

namespace AssetApi.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Asset> Assets => Set<Asset>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasIndex(u => u.Username).IsUnique();
            entity.Property(u => u.Role).HasMaxLength(50);
        });

        modelBuilder.Entity<Asset>(entity =>
        {
            entity.Property(a => a.Name).HasMaxLength(200).IsRequired();
            entity.Property(a => a.Type).HasMaxLength(100).IsRequired();
            entity.Property(a => a.SerialNumber).HasMaxLength(100);
            entity.Property(a => a.Location).HasMaxLength(200);
            entity.Property(a => a.Status).HasConversion<string>();
        });
    }
}
