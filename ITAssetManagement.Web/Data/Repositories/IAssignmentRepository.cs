using ITAssetManagement.Web.Models;

namespace ITAssetManagement.Web.Data.Repositories
{
    /// <summary>
    /// Zimmet repository arayüzü
    /// </summary>
    public interface IAssignmentRepository : IGenericRepository<Assignment>
    {
        /// <summary>
        /// Tüm zimmetleri ilişkili verilerle birlikte sorgulanabilir şekilde getirir
        /// </summary>
        /// <returns>Sorgulanabilir zimmet listesi</returns>
        IQueryable<Assignment> GetAllWithDetailsQueryable();

        /// <summary>
        /// Tüm zimmetleri ilişkili verilerle birlikte getirir
        /// </summary>
        /// <returns>Detaylı zimmet listesi</returns>
        Task<IEnumerable<Assignment>> GetAllWithDetailsAsync();

        /// <summary>
        /// ID'ye göre zimmeti ilişkili verilerle birlikte getirir
        /// </summary>
        /// <param name="id">Zimmet ID</param>
        /// <returns>Detaylı zimmet bilgisi veya null</returns>
        Task<Assignment?> GetByIdWithDetailsAsync(int id);

        /// <summary>
        /// Zimmeti siler
        /// </summary>
        /// <param name="entity">Silinecek zimmet</param>
        void Delete(Assignment entity);

        /// <summary>
        /// Değişiklikleri veritabanına kaydeder
        /// </summary>
        new Task SaveChangesAsync();
    }
}
