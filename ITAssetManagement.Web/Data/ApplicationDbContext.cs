using ITAssetManagement.Web.Models;
using ITAssetManagement.Web.Models.Email;
using Microsoft.EntityFrameworkCore;

namespace ITAssetManagement.Web.Data
{
    /// <summary>
    /// Uygulama veritabanı bağlamı
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        /// <summary>
        /// ApplicationDbContext constructor
        /// </summary>
        /// <param name="options">Veritabanı bağlantı ayarları</param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        /// <summary>
        /// Laptop veritabanı tablosu
        /// </summary>
        public DbSet<Laptop> Laptops { get; set; }

        /// <summary>
        /// Laptop fotoğrafları veritabanı tablosu
        /// </summary>
        public DbSet<LaptopPhoto> LaptopPhotos { get; set; }

        /// <summary>
        /// Laptop log kayıtları veritabanı tablosu
        /// </summary>
        public DbSet<LaptopLog> LaptopLogs { get; set; }

        /// <summary>
        /// Kullanıcılar veritabanı tablosu
        /// </summary>
        public DbSet<User> Users { get; set; }

        /// <summary>
        /// Zimmet kayıtları veritabanı tablosu
        /// </summary>
        public DbSet<Assignment> Assignments { get; set; }

        /// <summary>
        /// E-posta log kayıtları veritabanı tablosu
        /// </summary>
        public DbSet<EmailLog> EmailLogs { get; set; }

        /// <summary>
        /// Veritabanı model konfigürasyonlarını yapılandırır
        /// </summary>
        /// <param name="modelBuilder">Model oluşturucu</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Configure entity relationships if needed
            modelBuilder.Entity<Laptop>()
                .HasMany(l => l.Photos)
                .WithOne(p => p.Laptop)
                .HasForeignKey(p => p.LaptopId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Laptop>()
                .HasMany(l => l.Loglar)
                .WithOne(log => log.Laptop)
                .HasForeignKey(log => log.LaptopId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
