using ITAssetManagement.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace ITAssetManagement.Web.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Laptop> Laptops { get; set; }
        public DbSet<LaptopPhoto> LaptopPhotos { get; set; }
        public DbSet<LaptopLog> LaptopLogs { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<DeletedLaptop> DeletedLaptops { get; set; }

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
