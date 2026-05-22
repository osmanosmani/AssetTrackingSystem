using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AssetTrackingSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddSerialNumberUniqueIndexAndOfficeEquipmentSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SerialNumber",
                table: "Assets",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.InsertData(
                table: "Assets",
                columns: new[] { "Id", "AssetType", "Brand", "EmployeeUsername", "LocalPrice", "ModelName", "OfficeId", "PurchaseDate", "PurchasePriceUsd", "SerialNumber", "WarrantyExpirationDate" },
                values: new object[] { 5, "Office Equipment", "Canon", null, 4410m, "ImageClass MF455dw", 1, new DateTime(2025, 6, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 420m, "CAN-PRN-005", new DateTime(2028, 6, 3, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.CreateIndex(
                name: "IX_Assets_SerialNumber",
                table: "Assets",
                column: "SerialNumber",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Assets_SerialNumber",
                table: "Assets");

            migrationBuilder.DeleteData(
                table: "Assets",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.AlterColumn<string>(
                name: "SerialNumber",
                table: "Assets",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
