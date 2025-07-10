using ITAssetManagement.Web.Services.Interfaces;
using ITAssetManagement.Web.Models;
using ITAssetManagement.Web.Data.Repositories;
using ITAssetManagement.Web.Data;

namespace ITAssetManagement.Web.Services
{
    public class LaptopService : ILaptopService
    {
        private readonly ILaptopRepository _laptopRepository;
        private readonly ApplicationDbContext _context;

        public LaptopService(ILaptopRepository laptopRepository, ApplicationDbContext context)
        {
            _laptopRepository = laptopRepository;
            _context = context;
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
            var laptop = await _laptopRepository.GetByIdAsync(id);
            if (laptop == null)
                return false;

            // Silinen laptop'ı DeletedLaptops tablosuna kaydet
            var deletedLaptop = new DeletedLaptop
            {
                OriginalLaptopId = laptop.Id,
                EtiketNo = laptop.EtiketNo,
                Marka = laptop.Marka,
                Model = laptop.Model,
                Ozellikler = laptop.Ozellikler,
                Durum = laptop.Durum,
                KayitTarihi = laptop.KayitTarihi,
                SilinmeTarihi = DateTime.Now,
                // TODO: Kullanıcı sistemi eklendikten sonra aktif edilecek
                // SilenKullanici = currentUser.UserName,
                SilenKullanici = "System Admin", // Geçici olarak
                SilmeNedeni = !string.IsNullOrWhiteSpace(silmeNedeni) ? silmeNedeni : "Manuel silme işlemi"
            };

            _context.DeletedLaptops.Add(deletedLaptop);
            await _context.SaveChangesAsync();

            // Orijinal laptop'ı sil
            _laptopRepository.Remove(laptop);
            return await _laptopRepository.SaveChangesAsync();
        }
    }
}
