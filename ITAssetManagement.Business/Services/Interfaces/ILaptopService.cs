using ITAssetManagement.Core.Entities;

namespace ITAssetManagement.Business.Services.Interfaces
{
    public interface ILaptopService
    {
        Task<IEnumerable<Laptop>> GetAllLaptopsAsync();
        Task<Laptop?> GetLaptopByIdAsync(int id);
        Task<Laptop?> GetLaptopWithDetailsAsync(int id);
        Task<bool> CreateLaptopAsync(Laptop laptop);
        Task<bool> UpdateLaptopAsync(Laptop laptop);
        Task<bool> DeleteLaptopAsync(int id);
    }
}
