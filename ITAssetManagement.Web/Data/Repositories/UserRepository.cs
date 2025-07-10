using ITAssetManagement.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace ITAssetManagement.Web.Data.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<IEnumerable<User>> GetUsersByDepartmentAsync(string department)
        {
            return await _dbSet.Where(u => u.Department == department).ToListAsync();
        }
    }
}
