using ITAssetManagement.Web.Models;

namespace ITAssetManagement.Web.Data.Repositories
{
    /// <summary>
    /// Laptop repository arayüzü
    /// </summary>
    public interface ILaptopRepository : IGenericRepository<Laptop>
    {
        /// <summary>
        /// ID'ye göre laptop'u ilişkili verilerle birlikte getirir
        /// </summary>
        /// <param name="id">Laptop ID</param>
        /// <returns>Detaylı laptop bilgisi veya null</returns>
        Task<Laptop?> GetLaptopWithDetailsAsync(int id);

        /// <summary>
        /// Tüm laptop'ları ilişkili verilerle birlikte getirir
        /// </summary>
        /// <returns>Detaylı laptop listesi</returns>
        Task<IEnumerable<Laptop>> GetLaptopsWithDetailsAsync();

        /// <summary>
        /// Aktif durumdaki tüm laptop'ları getirir
        /// </summary>
        /// <returns>Aktif laptop listesi</returns>
        Task<IEnumerable<Laptop>> GetAllActiveLaptopsAsync();

        /// <summary>
        /// Silinmiş durumdaki tüm laptop'ları getirir
        /// </summary>
        /// <returns>Silinmiş laptop listesi</returns>
        Task<IEnumerable<Laptop>> GetAllDeletedLaptopsAsync();

        /// <summary>
        /// ID'ye göre laptop'u silinmiş olsa bile getirir
        /// </summary>
        /// <param name="id">Laptop ID</param>
        /// <returns>Laptop bilgisi veya null</returns>
        Task<Laptop?> GetLaptopByIdIncludingDeletedAsync(int id);
    }
}
