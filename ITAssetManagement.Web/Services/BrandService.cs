using ITAssetManagement.Web.Data.Repositories;
using ITAssetManagement.Web.Models;
using ITAssetManagement.Web.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ITAssetManagement.Web.Services
{
    /// <summary>
    /// Marka yönetimi servis implementasyonu
    /// </summary>
    public class BrandService : IBrandService
    {
        private readonly IGenericRepository<Brand> _brandRepository;
        private readonly IGenericRepository<Laptop> _laptopRepository;

        /// <summary>
        /// BrandService constructor
        /// </summary>
        /// <param name="brandRepository">Marka repository</param>
        /// <param name="laptopRepository">Laptop repository</param>
        public BrandService(
            IGenericRepository<Brand> brandRepository,
            IGenericRepository<Laptop> laptopRepository)
        {
            _brandRepository = brandRepository;
            _laptopRepository = laptopRepository;
        }

        /// <summary>
        /// Tüm aktif markaları getirir
        /// </summary>
        /// <returns>Aktif marka listesi</returns>
        public async Task<List<Brand>> GetAllActiveBrandsAsync()
        {
            var brands = await _brandRepository.GetAllAsync();
            return brands.Where(b => b.IsActive)
                        .OrderBy(b => b.Name)
                        .ToList();
        }

        /// <summary>
        /// Tüm markaları getirir (aktif ve pasif)
        /// </summary>
        /// <returns>Tüm marka listesi</returns>
        public async Task<List<Brand>> GetAllBrandsAsync()
        {
            var brands = await _brandRepository.GetAllAsync();
            return brands.OrderBy(b => b.Name).ToList();
        }

        /// <summary>
        /// ID'ye göre marka getirir
        /// </summary>
        /// <param name="id">Marka ID</param>
        /// <returns>Marka bilgisi</returns>
        public async Task<Brand?> GetBrandByIdAsync(int id)
        {
            return await _brandRepository.GetByIdAsync(id);
        }

        /// <summary>
        /// Yeni marka oluşturur
        /// </summary>
        /// <param name="brand">Oluşturulacak marka</param>
        /// <returns>İşlem başarılı ise true</returns>
        public async Task<bool> CreateBrandAsync(Brand brand)
        {
            if (brand == null || string.IsNullOrWhiteSpace(brand.Name))
                return false;

            // Aynı isimde marka var mı kontrol et
            var existingBrand = await GetBrandByNameAsync(brand.Name);
            if (existingBrand != null)
                return false;

            brand.CreatedDate = DateTime.Now;
            brand.IsActive = true;

            await _brandRepository.AddAsync(brand);
            return await _brandRepository.SaveChangesAsync();
        }

        /// <summary>
        /// Marka bilgilerini günceller
        /// </summary>
        /// <param name="brand">Güncellenecek marka</param>
        /// <returns>İşlem başarılı ise true</returns>
        public async Task<bool> UpdateBrandAsync(Brand brand)
        {
            if (brand == null || string.IsNullOrWhiteSpace(brand.Name))
                return false;

            // Aynı isimde başka marka var mı kontrol et
            var existingBrand = await GetBrandByNameAsync(brand.Name);
            if (existingBrand != null && existingBrand.Id != brand.Id)
                return false;

            _brandRepository.Update(brand);
            return await _brandRepository.SaveChangesAsync();
        }

        /// <summary>
        /// Marka adına göre arama yapar
        /// </summary>
        /// <param name="name">Aranacak marka adı</param>
        /// <returns>Bulunan marka</returns>
        public async Task<Brand?> GetBrandByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return null;

            var brands = await _brandRepository.GetAllAsync();
            return brands.FirstOrDefault(b => 
                b.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Markanın silinip silinemeyeceğini kontrol eder
        /// </summary>
        /// <param name="id">Marka ID</param>
        /// <returns>Silinebilir ise true</returns>
        public async Task<bool> CanDeleteBrandAsync(int id)
        {
            var laptops = await _laptopRepository.GetAllAsync();
            return !laptops.Any(l => l.BrandId == id);
        }

        /// <summary>
        /// Markayı pasif yapar (soft delete)
        /// </summary>
        /// <param name="id">Marka ID</param>
        /// <returns>İşlem başarılı ise true</returns>
        public async Task<bool> DeactivateBrandAsync(int id)
        {
            var brand = await GetBrandByIdAsync(id);
            if (brand == null)
                return false;

            brand.IsActive = false;
            return await UpdateBrandAsync(brand);
        }
    }
}
