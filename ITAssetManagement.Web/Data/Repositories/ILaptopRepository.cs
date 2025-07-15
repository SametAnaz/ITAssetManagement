using ITAssetManagement.Web.Models;

namespace ITAssetManagement.Web.Data.Repositories
{
    public interface ILaptopRepository : IGenericRepository<Laptop>
    {
        Task<Laptop?> GetLaptopWithDetailsAsync(int id);
        Task<IEnumerable<Laptop>> GetLaptopsWithDetailsAsync();
        Task<IEnumerable<Laptop>> GetAllActiveLaptopsAsync();
        Task<IEnumerable<Laptop>> GetAllDeletedLaptopsAsync();
        Task<Laptop?> GetLaptopByIdIncludingDeletedAsync(int id);
    }
}
