using ITAssetManagement.Web.Models;

namespace ITAssetManagement.Web.Services.Interfaces
{
    public interface ILaptopService
    {
        Task<IEnumerable<Laptop>> GetAllLaptopsAsync();
        Task<Laptop?> GetLaptopByIdAsync(int id);
        Task<Laptop?> GetLaptopWithDetailsAsync(int id);
        Task<bool> CreateLaptopAsync(Laptop laptop);
        Task<bool> UpdateLaptopAsync(Laptop laptop);
        Task<bool> DeleteLaptopAsync(int id, string? silmeNedeni = null);
        Task<IEnumerable<Laptop>> GetDeletedLaptopsAsync();
        Task<bool> RestoreLaptopAsync(int id);
        Task<IEnumerable<Laptop>> GetAvailableLaptopsAsync();
    }
}
