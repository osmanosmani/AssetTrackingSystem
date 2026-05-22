using Microsoft.EntityFrameworkCore;
using AssetTrackingSystem.Models;

namespace AssetTrackingSystem.Data;

public class AppDbContext : DbContext
{
    private const string ConnectionString =
        @"Server=(localdb)\mssqllocaldb;Database=AssetTrackingDb;Trusted_Connection=True;TrustServerCertificate=True";

    public DbSet<Asset> Assets { get; set; }

    public DbSet<ComputerAsset> ComputerAssets { get; set; }

    public DbSet<MobileAsset> MobileAssets { get; set; }

    public DbSet<Office> Offices { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(ConnectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // TPT mapping keeps common asset fields in Assets and category-specific fields in separate tables.
        modelBuilder.Entity<Asset>().ToTable("Assets");
        modelBuilder.Entity<ComputerAsset>().ToTable("ComputerAssets");
        modelBuilder.Entity<MobileAsset>().ToTable("MobileAssets");
        modelBuilder.Entity<Office>().ToTable("Offices");

        modelBuilder.Entity<Asset>()
            .Property(asset => asset.PurchasePriceUsd)
            .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<Asset>()
            .Property(asset => asset.LocalPrice)
            .HasColumnType("decimal(18,2)");

        // Seed data gives the demo a complete starting point after running Update-Database.
        modelBuilder.Entity<Office>().HasData(
            new Office { Id = 1, Name = "Sweden Office", Country = "Sweden", Currency = "SEK" },
            new Office { Id = 2, Name = "USA Office", Country = "USA", Currency = "USD" },
            new Office { Id = 3, Name = "Germany Office", Country = "Germany", Currency = "EUR" },
            new Office { Id = 4, Name = "Turkey Office", Country = "Turkey", Currency = "TRY" });

        modelBuilder.Entity<ComputerAsset>().HasData(
            new ComputerAsset
            {
                Id = 1,
                AssetType = "Laptop",
                ComputerType = "Laptop",
                Brand = "Dell",
                ModelName = "Latitude 5440",
                PurchaseDate = new DateTime(2024, 2, 10),
                PurchasePriceUsd = 1200,
                LocalPrice = 1104,
                SerialNumber = "DL-LAT-001",
                EmployeeUsername = "anna",
                WarrantyExpirationDate = new DateTime(2027, 2, 10),
                OfficeId = 3,
                Processor = "Intel Core i5",
                RamGb = 16,
                StorageGb = 512
            },
            new ComputerAsset
            {
                Id = 3,
                AssetType = "Desktop Computer",
                ComputerType = "Desktop",
                Brand = "HP",
                ModelName = "EliteDesk 800",
                PurchaseDate = new DateTime(2023, 8, 5),
                PurchasePriceUsd = 850,
                LocalPrice = 8925,
                SerialNumber = "HP-ED800-003",
                EmployeeUsername = null,
                WarrantyExpirationDate = new DateTime(2026, 8, 5),
                OfficeId = 1,
                Processor = "Intel Core i7",
                RamGb = 32,
                StorageGb = 1024
            });

        modelBuilder.Entity<MobileAsset>().HasData(
            new MobileAsset
            {
                Id = 2,
                AssetType = "Mobile Phone",
                MobileType = "iPhone",
                Brand = "Apple",
                ModelName = "iPhone 15",
                PurchaseDate = new DateTime(2025, 1, 20),
                PurchasePriceUsd = 999,
                LocalPrice = 999,
                SerialNumber = "APL-IP15-002",
                EmployeeUsername = "michael",
                WarrantyExpirationDate = new DateTime(2028, 1, 20),
                OfficeId = 2,
                ImeiNumber = "356789012345678",
                PhoneNumber = "+1 555 0100"
            },
            new MobileAsset
            {
                Id = 4,
                AssetType = "Tablet",
                MobileType = "Tablet",
                Brand = "Samsung",
                ModelName = "Galaxy Tab S9",
                PurchaseDate = new DateTime(2024, 11, 15),
                PurchasePriceUsd = 750,
                LocalPrice = 24000,
                SerialNumber = "SMS-TABS9-004",
                EmployeeUsername = "ayla",
                WarrantyExpirationDate = new DateTime(2027, 11, 15),
                OfficeId = 4,
                ImeiNumber = null,
                PhoneNumber = null
            });
    }
}
