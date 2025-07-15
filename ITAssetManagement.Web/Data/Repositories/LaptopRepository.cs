using ITAssetManagement.Web.Models;
using ITAssetManagement.Web.Data;
using Microsoft.EntityFrameworkCore;

namespace ITAssetManagement.Web.Data.Repositories
{
    public class LaptopRepository : GenericRepository<Laptop>, ILaptopRepository
    {
        public LaptopRepository(ApplicationDbContext context) : base(context)
        {
        }

        // Override GetAllAsync to return only active laptops
        public new async Task<IEnumerable<Laptop>> GetAllAsync()
        {
            return await _context.Laptops
                .Where(l => l.IsActive)
                .ToListAsync();
        }

        // Override GetByIdAsync to return only active laptops
        public new async Task<Laptop?> GetByIdAsync(int id)
        {
            return await _context.Laptops
                .FirstOrDefaultAsync(l => l.Id == id && l.IsActive);
        }

        public async Task<Laptop?> GetLaptopWithDetailsAsync(int id)
        {
            return await _context.Laptops
                .Include(l => l.Photos)
                .Include(l => l.Loglar)
                .FirstOrDefaultAsync(l => l.Id == id && l.IsActive);
        }

        public async Task<IEnumerable<Laptop>> GetLaptopsWithDetailsAsync()
        {
            return await _context.Laptops
                .Include(l => l.Photos)
                .Include(l => l.Loglar)
                .Where(l => l.IsActive)
                .ToListAsync();
        }

        public async Task<IEnumerable<Laptop>> GetAllActiveLaptopsAsync()
        {
            return await _context.Laptops
                .Where(l => l.IsActive)
                .ToListAsync();
        }

        public async Task<IEnumerable<Laptop>> GetAllDeletedLaptopsAsync()
        {
            return await _context.Laptops
                .Where(l => !l.IsActive)
                .ToListAsync();
        }

        public async Task<Laptop?> GetLaptopByIdIncludingDeletedAsync(int id)
        {
            return await _context.Laptops
                .FirstOrDefaultAsync(l => l.Id == id);
        }
    }
}
