using ITAssetManagement.Core.Entities;

namespace ITAssetManagement.DataAccess.Repositories.Interfaces
{
    public interface ILaptopRepository : IGenericRepository<Laptop>
    {
        Task<Laptop?> GetLaptopWithDetailsAsync(int id);
        Task<IEnumerable<Laptop>> GetLaptopsWithDetailsAsync();
    }
}
