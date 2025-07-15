using ITAssetManagement.Web.Services.Interfaces;
using ITAssetManagement.Web.Models;
using ITAssetManagement.Web.Data.Repositories;

namespace ITAssetManagement.Web.Services
{
    public class LaptopService : ILaptopService
    {
        private readonly ILaptopRepository _laptopRepository;

        public LaptopService(ILaptopRepository laptopRepository)
        {
            _laptopRepository = laptopRepository;
        }

        public async Task<IEnumerable<Laptop>> GetAllLaptopsAsync()
        {
            return await _laptopRepository.GetAllAsync();
        }

        public async Task<Laptop?> GetLaptopByIdAsync(int id)
        {
            return await _laptopRepository.GetByIdAsync(id);
        }

        public async Task<Laptop?> GetLaptopWithDetailsAsync(int id)
        {
            return await _laptopRepository.GetLaptopWithDetailsAsync(id);
        }

        public async Task<bool> CreateLaptopAsync(Laptop laptop)
        {
            if (laptop == null)
                return false;

            await _laptopRepository.AddAsync(laptop);
            return await _laptopRepository.SaveChangesAsync();
        }

        public async Task<bool> UpdateLaptopAsync(Laptop laptop)
        {
            if (laptop == null)
                return false;

            _laptopRepository.Update(laptop);
            return await _laptopRepository.SaveChangesAsync();
        }

        public async Task<bool> DeleteLaptopAsync(int id, string? silmeNedeni = null)
        {
            var laptop = await _laptopRepository.GetLaptopByIdIncludingDeletedAsync(id);
            if (laptop == null)
                return false;

            // Soft delete - laptop'ı veritabanından silmeyip sadece IsActive = false yapıyoruz
            laptop.IsActive = false;
            laptop.SilinmeTarihi = DateTime.Now;
            laptop.SilmeNedeni = !string.IsNullOrWhiteSpace(silmeNedeni) ? silmeNedeni : "Manuel silme işlemi";
            // TODO: Kullanıcı sistemi eklendikten sonra aktif edilecek
            // laptop.SilenKullanici = currentUser.UserName;
            laptop.SilenKullanici = "System Admin"; // Geçici olarak

            _laptopRepository.Update(laptop);
            return await _laptopRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<Laptop>> GetDeletedLaptopsAsync()
        {
            return await _laptopRepository.GetAllDeletedLaptopsAsync();
        }

        public async Task<bool> RestoreLaptopAsync(int id)
        {
            var laptop = await _laptopRepository.GetLaptopByIdIncludingDeletedAsync(id);
            if (laptop == null || laptop.IsActive)
                return false;

            laptop.IsActive = true;
            laptop.SilinmeTarihi = null;
            laptop.SilmeNedeni = null;
            laptop.SilenKullanici = null;

            _laptopRepository.Update(laptop);
            return await _laptopRepository.SaveChangesAsync();
        }
    }
}
