using ITAssetManagement.Web.Models;
using System.Linq;

namespace ITAssetManagement.Web.Services.Interfaces
{
    public interface ILaptopService
    {
        Task<IEnumerable<Laptop>> GetAllLaptopsAsync();
        IQueryable<Laptop> GetAllLaptopsQueryable();
        Task<Laptop?> GetLaptopByIdAsync(int id);
        Task<Laptop?> GetLaptopWithDetailsAsync(int id);
        Task<bool> CreateLaptopAsync(Laptop laptop);
        Task<bool> UpdateLaptopAsync(Laptop laptop);
        Task<bool> DeleteLaptopAsync(int id, string? silmeNedeni = null);
        Task<IEnumerable<Laptop>> GetDeletedLaptopsAsync();
        Task<bool> RestoreLaptopAsync(int id);
        Task<IEnumerable<Laptop>> GetAvailableLaptopsAsync();
        Task<IEnumerable<Laptop>> SearchLaptopsAsync(string searchTerm);
        IQueryable<Laptop> SearchLaptopsQueryable(string searchTerm);
    }
}
