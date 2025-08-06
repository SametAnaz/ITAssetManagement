using ITAssetManagement.Web.Models;

namespace ITAssetManagement.Web.Services.Interfaces
{
    /// <summary>
    /// Marka yönetimi için servis arayüzü
    /// </summary>
    public interface IBrandService
    {
        /// <summary>
        /// Tüm aktif markaları getirir
        /// </summary>
        /// <returns>Aktif marka listesi</returns>
        Task<List<Brand>> GetAllActiveBrandsAsync();

        /// <summary>
        /// Tüm markaları getirir (aktif ve pasif)
        /// </summary>
        /// <returns>Tüm marka listesi</returns>
        Task<List<Brand>> GetAllBrandsAsync();

        /// <summary>
        /// ID'ye göre marka getirir
        /// </summary>
        /// <param name="id">Marka ID</param>
        /// <returns>Marka bilgisi</returns>
        Task<Brand?> GetBrandByIdAsync(int id);

        /// <summary>
        /// Yeni marka oluşturur
        /// </summary>
        /// <param name="brand">Oluşturulacak marka</param>
        /// <returns>İşlem başarılı ise true</returns>
        Task<bool> CreateBrandAsync(Brand brand);

        /// <summary>
        /// Marka bilgilerini günceller
        /// </summary>
        /// <param name="brand">Güncellenecek marka</param>
        /// <returns>İşlem başarılı ise true</returns>
        Task<bool> UpdateBrandAsync(Brand brand);

        /// <summary>
        /// Marka adına göre arama yapar
        /// </summary>
        /// <param name="name">Aranacak marka adı</param>
        /// <returns>Bulunan marka</returns>
        Task<Brand?> GetBrandByNameAsync(string name);

        /// <summary>
        /// Markanın silinip silinemeyeceğini kontrol eder
        /// </summary>
        /// <param name="id">Marka ID</param>
        /// <returns>Silinebilir ise true</returns>
        Task<bool> CanDeleteBrandAsync(int id);

        /// <summary>
        /// Markayı pasif yapar (soft delete)
        /// </summary>
        /// <param name="id">Marka ID</param>
        /// <returns>İşlem başarılı ise true</returns>
        Task<bool> DeactivateBrandAsync(int id);
    }
}
