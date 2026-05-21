using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AssetTrackingSystem.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Offices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Offices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Assets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssetType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Brand = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModelName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PurchaseDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PurchasePriceUsd = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LocalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SerialNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmployeeUsername = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WarrantyExpirationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OfficeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Assets_Offices_OfficeId",
                        column: x => x.OfficeId,
                        principalTable: "Offices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Offices",
                columns: new[] { "Id", "Country", "Currency", "Name" },
                values: new object[,]
                {
                    { 1, "Sweden", "SEK", "Sweden Office" },
                    { 2, "USA", "USD", "USA Office" },
                    { 3, "Germany", "EUR", "Germany Office" },
                    { 4, "Turkey", "TRY", "Turkey Office" }
                });

            migrationBuilder.InsertData(
                table: "Assets",
                columns: new[] { "Id", "AssetType", "Brand", "EmployeeUsername", "LocalPrice", "ModelName", "OfficeId", "PurchaseDate", "PurchasePriceUsd", "SerialNumber", "WarrantyExpirationDate" },
                values: new object[,]
                {
                    { 1, "Laptop", "Dell", "anna", 1104m, "Latitude 5440", 3, new DateTime(2024, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 1200m, "DL-LAT-001", new DateTime(2027, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, "Mobile Phone", "Apple", "michael", 999m, "iPhone 15", 2, new DateTime(2025, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 999m, "APL-IP15-002", new DateTime(2028, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, "Desktop Computer", "HP", null, 8925m, "EliteDesk 800", 1, new DateTime(2023, 8, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 850m, "HP-ED800-003", new DateTime(2026, 8, 5, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Assets_OfficeId",
                table: "Assets",
                column: "OfficeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Assets");

            migrationBuilder.DropTable(
                name: "Offices");
        }
    }
}
