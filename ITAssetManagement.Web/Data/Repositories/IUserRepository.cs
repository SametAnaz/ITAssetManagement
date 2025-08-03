using ITAssetManagement.Web.Models;

namespace ITAssetManagement.Web.Data.Repositories
{
    /// <summary>
    /// Kullanıcı repository arayüzü
    /// </summary>
    public interface IUserRepository : IGenericRepository<User>
    {
        /// <summary>
        /// E-posta adresine göre kullanıcı getirir
        /// </summary>
        /// <param name="email">Kullanıcı e-posta adresi</param>
        /// <returns>Kullanıcı bilgisi veya null</returns>
        Task<User?> GetUserByEmailAsync(string email);

        /// <summary>
        /// Departmana göre kullanıcıları filtreler
        /// </summary>
        /// <param name="department">Departman adı</param>
        /// <returns>Filtrelenmiş kullanıcı listesi</returns>
        Task<IEnumerable<User>> GetUsersByDepartmentAsync(string department);

        /// <summary>
        /// ID'ye göre kullanıcıyı zimmetleriyle birlikte getirir
        /// </summary>
        /// <param name="id">Kullanıcı ID</param>
        /// <returns>Detaylı kullanıcı bilgisi veya null</returns>
        Task<User?> GetUserWithAssignmentsAsync(int id);
    }
}
