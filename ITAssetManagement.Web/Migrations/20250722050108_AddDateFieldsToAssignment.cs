using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ITAssetManagement.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddDateFieldsToAssignment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeletedLaptops");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Assignments",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<DateTime>(
                name: "AssignmentDate",
                table: "Assignments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ReturnDate",
                table: "Assignments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_UserId",
                table: "Assignments",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Assignments_Users_UserId",
                table: "Assignments",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assignments_Users_UserId",
                table: "Assignments");

            migrationBuilder.DropIndex(
                name: "IX_Assignments_UserId",
                table: "Assignments");

            migrationBuilder.DropColumn(
                name: "AssignmentDate",
                table: "Assignments");

            migrationBuilder.DropColumn(
                name: "ReturnDate",
                table: "Assignments");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Assignments",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "DeletedLaptops",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Durum = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EtiketNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    KayitTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Marka = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Model = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    OriginalLaptopId = table.Column<int>(type: "int", nullable: false),
                    Ozellikler = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    SilenKullanici = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SilinmeTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SilmeNedeni = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeletedLaptops", x => x.Id);
                });
        }
    }
}
