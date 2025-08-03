using ITAssetManagement.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace ITAssetManagement.Web.Data.Repositories
{
    /// <summary>
    /// Kullanıcı repository sınıfı implementasyonu
    /// </summary>
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        /// <summary>
        /// UserRepository constructor
        /// </summary>
        /// <param name="context">Veritabanı bağlamı</param>
        public UserRepository(ApplicationDbContext context) : base(context)
        {
        }

        /// <summary>
        /// E-posta adresine göre kullanıcı getirir
        /// </summary>
        /// <param name="email">Kullanıcı e-posta adresi</param>
        /// <returns>Kullanıcı bilgisi veya null</returns>
        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.Email == email);
        }

        /// <summary>
        /// Departmana göre kullanıcıları filtreler
        /// </summary>
        /// <param name="department">Departman adı</param>
        /// <returns>Filtrelenmiş kullanıcı listesi</returns>
        public async Task<IEnumerable<User>> GetUsersByDepartmentAsync(string department)
        {
            return await _dbSet.Where(u => u.Department == department).ToListAsync();
        }

        /// <summary>
        /// ID'ye göre kullanıcıyı zimmetleriyle birlikte getirir
        /// </summary>
        /// <param name="id">Kullanıcı ID</param>
        /// <returns>Detaylı kullanıcı bilgisi veya null</returns>
        public async Task<User?> GetUserWithAssignmentsAsync(int id)
        {
            return await _dbSet
                .Include(u => u.Assignments)
                    .ThenInclude(a => a.Laptop)
                .FirstOrDefaultAsync(u => u.Id == id);
        }
    }
}
