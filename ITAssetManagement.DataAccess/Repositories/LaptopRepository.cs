using ITAssetManagement.Core.Entities;
using ITAssetManagement.DataAccess.Context;
using ITAssetManagement.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ITAssetManagement.DataAccess.Repositories
{
    public class LaptopRepository : GenericRepository<Laptop>, ILaptopRepository
    {
        public LaptopRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Laptop?> GetLaptopWithDetailsAsync(int id)
        {
            return await _context.Laptops
                .Include(l => l.Photos)
                .Include(l => l.Loglar)
                .FirstOrDefaultAsync(l => l.Id == id);
        }

        public async Task<IEnumerable<Laptop>> GetLaptopsWithDetailsAsync()
        {
            return await _context.Laptops
                .Include(l => l.Photos)
                .Include(l => l.Loglar)
                .ToListAsync();
        }
    }
}
