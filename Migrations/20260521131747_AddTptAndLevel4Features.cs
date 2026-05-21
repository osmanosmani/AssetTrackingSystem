using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AssetTrackingSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddTptAndLevel4Features : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssetCategory",
                table: "Assets");

            migrationBuilder.CreateTable(
                name: "ComputerAssets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    ComputerType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Processor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RamGb = table.Column<int>(type: "int", nullable: true),
                    StorageGb = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComputerAssets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ComputerAssets_Assets_Id",
                        column: x => x.Id,
                        principalTable: "Assets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MobileAssets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    MobileType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImeiNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MobileAssets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MobileAssets_Assets_Id",
                        column: x => x.Id,
                        principalTable: "Assets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "ComputerAssets",
                columns: new[] { "Id", "ComputerType", "Processor", "RamGb", "StorageGb" },
                values: new object[,]
                {
                    { 1, "Laptop", "Intel Core i5", 16, 512 },
                    { 3, "Desktop", "Intel Core i7", 32, 1024 }
                });

            migrationBuilder.InsertData(
                table: "MobileAssets",
                columns: new[] { "Id", "ImeiNumber", "MobileType", "PhoneNumber" },
                values: new object[,]
                {
                    { 2, "356789012345678", "iPhone", "+1 555 0100" },
                    { 4, null, "Tablet", null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ComputerAssets");

            migrationBuilder.DropTable(
                name: "MobileAssets");

            migrationBuilder.DeleteData(
                table: "Assets",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Assets",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Assets",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Assets",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.AddColumn<string>(
                name: "AssetCategory",
                table: "Assets",
                type: "nvarchar(8)",
                maxLength: 8,
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "Assets",
                columns: new[] { "Id", "AssetCategory", "AssetType", "Brand", "EmployeeUsername", "LocalPrice", "ModelName", "OfficeId", "PurchaseDate", "PurchasePriceUsd", "SerialNumber", "WarrantyExpirationDate" },
                values: new object[,]
                {
                    { 1, "Computer", "Laptop", "Dell", "anna", 1104m, "Latitude 5440", 3, new DateTime(2024, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 1200m, "DL-LAT-001", new DateTime(2027, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, "Mobile", "Mobile Phone", "Apple", "michael", 999m, "iPhone 15", 2, new DateTime(2025, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 999m, "APL-IP15-002", new DateTime(2028, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, "Computer", "Desktop Computer", "HP", null, 8925m, "EliteDesk 800", 1, new DateTime(2023, 8, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 850m, "HP-ED800-003", new DateTime(2026, 8, 5, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, "Mobile", "Tablet", "Samsung", "ayla", 24000m, "Galaxy Tab S9", 4, new DateTime(2024, 11, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 750m, "SMS-TABS9-004", new DateTime(2027, 11, 15, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });
        }
    }
}
