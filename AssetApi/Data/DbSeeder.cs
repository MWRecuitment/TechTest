using AssetApi.Models;

namespace AssetApi.Data;

public static class DbSeeder
{
    public static void Seed(AppDbContext context)
    {
        if (context.Users.Any()) return;

        context.Users.AddRange(
            new User
            {
                Username = "admin",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
                Role = "Admin"
            },
            new User
            {
                Username = "viewer",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Viewer123!"),
                Role = "Viewer"
            }
        );

        context.Assets.AddRange(
            new Asset
            {
                Name = "Dell Latitude 5540",
                Type = "Laptop",
                SerialNumber = "SN-LAP-001",
                Location = "Head Office - Floor 2",
                Status = AssetStatus.Active,
                PurchasedAt = new DateTime(2023, 3, 15, 0, 0, 0, DateTimeKind.Utc)
            },
            new Asset
            {
                Name = "HP EliteDesk 800",
                Type = "Desktop",
                SerialNumber = "SN-DSK-002",
                Location = "Branch Office A",
                Status = AssetStatus.Active,
                PurchasedAt = new DateTime(2022, 11, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new Asset
            {
                Name = "Cisco Catalyst 2960",
                Type = "Network Switch",
                SerialNumber = "SN-NET-003",
                Location = "Server Room",
                Status = AssetStatus.Active,
                PurchasedAt = new DateTime(2021, 6, 20, 0, 0, 0, DateTimeKind.Utc)
            },
            new Asset
            {
                Name = "Canon imageRUNNER 2625",
                Type = "Printer",
                SerialNumber = "SN-PRN-004",
                Location = "Head Office - Floor 1",
                Status = AssetStatus.InMaintenance,
                PurchasedAt = new DateTime(2020, 8, 5, 0, 0, 0, DateTimeKind.Utc)
            },
            new Asset
            {
                Name = "ThinkPad X1 Carbon",
                Type = "Laptop",
                SerialNumber = "SN-LAP-005",
                Location = "Remote",
                Status = AssetStatus.Retired,
                PurchasedAt = new DateTime(2019, 1, 12, 0, 0, 0, DateTimeKind.Utc)
            }
        );

        context.SaveChanges();
    }
}
