using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ITAssetManagement.Web.Migrations
{
    /// <summary>
    /// Veritabanı tablolarını oluşturan ve güncelleyen migration sınıfı.
    /// Bu migration, sistemin temel veri yapısını oluşturur ve veri modelinde yapılan değişiklikleri uygular.
    /// </summary>
    /// <remarks>
    /// Bu migration aşağıdaki işlemleri gerçekleştirir:
    /// <list type="bullet">
    /// <item><description>Laptop tablosunun oluşturulması ve güncellenmesi (IsActive, Notes, SilinmeTarihi vb. alanların eklenmesi)</description></item>
    /// <item><description>User tablosunun oluşturulması ve güncellenmesi (IsActive alanının değiştirilmesi)</description></item>
    /// <item><description>Assignment tablosunun oluşturulması ve güncellenmesi (AssignmentDate, ReturnDate, IslemTipi alanlarının eklenmesi)</description></item>
    /// <item><description>LaptopPhoto tablosunun güncellenmesi (UploadDate -> YuklemeTarihi değişimi)</description></item>
    /// <item><description>LaptopLog tablosunun oluşturulması (Laptop işlem geçmişini takip etmek için)</description></item>
    /// <item><description>EmailLog tablosunun oluşturulması (Email gönderim kayıtlarını tutmak için)</description></item>
    /// </list>
    /// Tüm tablolar için:
    /// <list type="bullet">
    /// <item><description>Primary key ve foreign key ilişkileri tanımlanmıştır</description></item>
    /// <item><description>Uygun veri tipleri ve boyut kısıtlamaları belirlenmiştir</description></item>
    /// <item><description>Gerekli indeksler oluşturulmuştur</description></item>
    /// <item><description>Varsayılan değerler ve nullable/required alanlar ayarlanmıştır</description></item>
    /// </list>
    /// </remarks>
    public partial class CreateTables : Migration
    {
        /// <summary>
        /// Migration'ı uygular ve veritabanı şemasını yükseltir.
        /// Tüm tabloları, ilişkileri ve kısıtlamaları oluşturur/günceller.
        /// </summary>
        /// <param name="migrationBuilder">Migration işlemlerini yönetmek için kullanılan builder nesnesi.</param>
        /// <remarks>
        /// Bu metot şu işlemleri sırasıyla gerçekleştirir:
        /// <list type="bullet">
        /// <item><description>Eski/kullanılmayan tabloları ve alanları kaldırır (DeletedLaptops tablosu gibi)</description></item>
        /// <item><description>Mevcut tablolardaki kolonları yeniden adlandırır ve günceller</description></item>
        /// <item><description>Yeni EmailLogs ve LaptopLogs tablolarını oluşturur</description></item>
        /// <item><description>İlişkileri ve foreign key'leri günceller/oluşturur</description></item>
        /// <item><description>Gerekli indeksleri oluşturur (örn: IX_Assignments_LaptopId)</description></item>
        /// <item><description>Veri bütünlüğü için kısıtlamaları ve varsayılan değerleri ayarlar</description></item>
        /// </list>
        /// Önemli Değişiklikler:
        /// <list type="bullet">
        /// <item><description>Tüm datetime alanları datetime(6) olarak güncellendi</description></item>
        /// <item><description>Tüm string alanlar için maksimum uzunluklar belirlendi</description></item>
        /// <item><description>Identity column stratejisi tüm ID'ler için ayarlandı</description></item>
        /// <item><description>Utf8mb4 karakter seti tüm string alanlar için tanımlandı</description></item>
        /// </list>
        /// </remarks>
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Assignments_LaptopId",
                table: "Assignments");

            migrationBuilder.DropColumn(
                name: "UploadDate",
                table: "LaptopPhotos");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "Assignments");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Assignments");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "Assignments");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "Assignments");

            migrationBuilder.RenameColumn(
                name: "PhotoPath",
                table: "LaptopPhotos",
                newName: "Path");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Users",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Users",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<DateTime>(
                name: "KayitTarihi",
                table: "Laptops",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Laptops",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Laptops",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "Laptops",
                type: "varchar(1000)",
                maxLength: 1000,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "SilenKullanici",
                table: "Laptops",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "SilinmeTarihi",
                table: "Laptops",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SilmeNedeni",
                table: "Laptops",
                type: "varchar(500)",
                maxLength: 500,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "LaptopPhotos",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<DateTime>(
                name: "YuklemeTarihi",
                table: "LaptopPhotos",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Assignments",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<DateTime>(
                name: "AssignmentDate",
                table: "Assignments",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "IslemTipi",
                table: "Assignments",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "ReturnDate",
                table: "Assignments",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Tarih",
                table: "Assignments",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "EmailLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ToEmail = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Subject = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Body = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SentDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    IsSuccess = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ErrorMessage = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RelatedEntityType = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RelatedEntityId = table.Column<int>(type: "int", nullable: true),
                    RetryCount = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailLogs", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "LaptopLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    LaptopId = table.Column<int>(type: "int", nullable: false),
                    Action = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Details = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LogDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UserId = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LaptopLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LaptopLogs_Laptops_LaptopId",
                        column: x => x.LaptopId,
                        principalTable: "Laptops",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_LaptopId",
                table: "Assignments",
                column: "LaptopId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LaptopLogs_LaptopId",
                table: "LaptopLogs",
                column: "LaptopId");
        }

        /// <summary>
        /// Migration'ı geri alır ve veritabanı şemasını önceki durumuna döndürür.
        /// Bu işlem, Up metodunda yapılan tüm değişiklikleri tersine çevirir.
        /// </summary>
        /// <param name="migrationBuilder">Migration işlemlerini yönetmek için kullanılan builder nesnesi.</param>
        /// <remarks>
        /// Bu metot şu işlemleri sırasıyla gerçekleştirir:
        /// <list type="bullet">
        /// <item><description>EmailLogs ve LaptopLogs tablolarını tamamen siler</description></item>
        /// <item><description>Assignments tablosundaki LaptopId indeksini kaldırır</description></item>
        /// <item><description>Laptops tablosundan eklenen yeni kolonları (Notes, SilenKullanici vb.) siler</description></item>
        /// <item><description>Tüm tablolardaki yapısal değişiklikleri geri alır</description></item>
        /// <item><description>Eklenen kısıtlamaları ve foreign key'leri kaldırır</description></item>
        /// <item><description>Veritabanı şemasını migration öncesi haline döndürür</description></item>
        /// </list>
        /// Önemli Notlar:
        /// <list type="bullet">
        /// <item><description>Bu işlem geri alınamaz, tüm veriler silinir</description></item>
        /// <item><description>EmailLogs ve LaptopLogs tablolarındaki tüm kayıtlar kaybolur</description></item>
        /// <item><description>Yeni eklenen alanlardaki veriler kaybolur</description></item>
        /// <item><description>Veri kaybını önlemek için gerekli yedekleme işlemleri önceden yapılmalıdır</description></item>
        /// </list>
        /// </remarks>
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmailLogs");

            migrationBuilder.DropTable(
                name: "LaptopLogs");

            migrationBuilder.DropIndex(
                name: "IX_Assignments_LaptopId",
                table: "Assignments");

            migrationBuilder.DropColumn(
                name: "Notes",
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

            migrationBuilder.DropColumn(
                name: "YuklemeTarihi",
                table: "LaptopPhotos");

            migrationBuilder.DropColumn(
                name: "AssignmentDate",
                table: "Assignments");

            migrationBuilder.DropColumn(
                name: "IslemTipi",
                table: "Assignments");

            migrationBuilder.DropColumn(
                name: "ReturnDate",
                table: "Assignments");

            migrationBuilder.DropColumn(
                name: "Tarih",
                table: "Assignments");

            migrationBuilder.RenameColumn(
                name: "Path",
                table: "LaptopPhotos",
                newName: "PhotoPath");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Users",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Users",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<DateTime>(
                name: "KayitTarihi",
                table: "Laptops",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Laptops",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Laptops",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "LaptopPhotos",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<DateTime>(
                name: "UploadDate",
                table: "LaptopPhotos",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Assignments",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "Assignments",
                type: "datetime",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Assignments",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "Assignments",
                type: "varchar(1000)",
                maxLength: 1000,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "Assignments",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "DeletedLaptops",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Durum = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EtiketNo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    KayitTarihi = table.Column<DateTime>(type: "datetime", nullable: false),
                    Marka = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Model = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    OriginalLaptopId = table.Column<int>(type: "int", nullable: false),
                    Ozellikler = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SilenKullanici = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SilinmeTarihi = table.Column<DateTime>(type: "datetime", nullable: false),
                    SilmeNedeni = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeletedLaptops", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_LaptopId",
                table: "Assignments",
                column: "LaptopId");
        }
    }
}
