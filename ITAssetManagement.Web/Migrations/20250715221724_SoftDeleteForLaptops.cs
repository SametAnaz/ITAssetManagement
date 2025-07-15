using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ITAssetManagement.Web.Migrations
{
    /// <inheritdoc />
    public partial class SoftDeleteForLaptops : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Laptops",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "SilenKullanici",
                table: "Laptops",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "SilinmeTarihi",
                table: "Laptops",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SilmeNedeni",
                table: "Laptops",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Laptops");

            migrationBuilder.DropColumn(
                name: "SilenKullanici",
                table: "Laptops");

            migrationBuilder.DropColumn(
                name: "SilinmeTarihi",
                table: "Laptops");

            migrationBuilder.DropColumn(
                name: "SilmeNedeni",
                table: "Laptops");
        }
    }
}
