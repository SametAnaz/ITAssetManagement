using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ITAssetManagement.Web.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUnnecessaryColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Aciklama",
                table: "Assignments");

            migrationBuilder.DropColumn(
                name: "ImzaYolu",
                table: "Assignments");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Aciklama",
                table: "Assignments",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImzaYolu",
                table: "Assignments",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);
        }
    }
}
