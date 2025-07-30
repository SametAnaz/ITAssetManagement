using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ITAssetManagement.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddNotesToLaptop : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Assignments_LaptopId",
                table: "Assignments");

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "Laptops",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_LaptopId",
                table: "Assignments",
                column: "LaptopId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Assignments_LaptopId",
                table: "Assignments");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "Laptops");

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_LaptopId",
                table: "Assignments",
                column: "LaptopId");
        }
    }
}
