using ITAssetManagement.Web.Models;
using System.Linq;

namespace ITAssetManagement.Web.Services.Interfaces
{
    /// <summary>
    /// Laptop işlemlerini yöneten servis arayüzü
    /// </summary>
    public interface ILaptopService
    {
        /// <summary>
        /// Tüm laptopları getirir
        /// </summary>
        /// <returns>Laptop listesi</returns>
        Task<IEnumerable<Laptop>> GetAllLaptopsAsync();

        /// <summary>
        /// Tüm laptopları sorgulanabilir şekilde getirir
        /// </summary>
        /// <returns>Sorgulanabilir laptop listesi</returns>
        IQueryable<Laptop> GetAllLaptopsQueryable();

        /// <summary>
        /// ID'ye göre laptop getirir
        /// </summary>
        /// <param name="id">Laptop ID</param>
        /// <returns>Laptop nesnesi veya null</returns>
        Task<Laptop?> GetLaptopByIdAsync(int id);

        /// <summary>
        /// ID'ye göre laptop ve ilişkili detaylarını getirir
        /// </summary>
        /// <param name="id">Laptop ID</param>
        /// <returns>Detaylı laptop nesnesi veya null</returns>
        Task<Laptop?> GetLaptopWithDetailsAsync(int id);

        /// <summary>
        /// Yeni laptop oluşturur
        /// </summary>
        /// <param name="laptop">Laptop nesnesi</param>
        /// <returns>İşlem başarılı ise true</returns>
        Task<bool> CreateLaptopAsync(Laptop laptop);

        /// <summary>
        /// Laptop bilgilerini günceller
        /// </summary>
        /// <param name="laptop">Güncellenecek laptop nesnesi</param>
        /// <returns>İşlem başarılı ise true</returns>
        Task<bool> UpdateLaptopAsync(Laptop laptop);

        /// <summary>
        /// Laptop'u siler (soft delete)
        /// </summary>
        /// <param name="id">Laptop ID</param>
        /// <param name="silmeNedeni">Silme nedeni</param>
        /// <returns>İşlem başarılı ise true</returns>
        Task<bool> DeleteLaptopAsync(int id, string? silmeNedeni = null);

        /// <summary>
        /// Silinmiş laptopları getirir
        /// </summary>
        /// <returns>Silinmiş laptop listesi</returns>
        Task<IEnumerable<Laptop>> GetDeletedLaptopsAsync();

        /// <summary>
        /// Silinmiş laptopları sorgulanabilir şekilde getirir
        /// </summary>
        /// <returns>Sorgulanabilir silinmiş laptop listesi</returns>
        IQueryable<Laptop> GetDeletedLaptopsQueryable();

        /// <summary>
        /// Silinmiş laptop'u geri yükler
        /// </summary>
        /// <param name="id">Laptop ID</param>
        /// <returns>İşlem başarılı ise true</returns>
        Task<bool> RestoreLaptopAsync(int id);

        /// <summary>
        /// Zimmetlenmemiş laptopları getirir
        /// </summary>
        /// <returns>Müsait laptop listesi</returns>
        Task<IEnumerable<Laptop>> GetAvailableLaptopsAsync();

        /// <summary>
        /// Laptop'ları arama terimlerine göre filtreler
        /// </summary>
        /// <param name="searchTerm">Arama terimi</param>
        /// <returns>Filtrelenmiş laptop listesi</returns>
        Task<IEnumerable<Laptop>> SearchLaptopsAsync(string searchTerm);

        /// <summary>
        /// Laptop'ları arama terimlerine göre sorgulanabilir şekilde filtreler
        /// </summary>
        /// <param name="searchTerm">Arama terimi</param>
        /// <returns>Sorgulanabilir filtrelenmiş laptop listesi</returns>
        IQueryable<Laptop> SearchLaptopsQueryable(string searchTerm);

        /// <summary>
        /// Silinmiş laptop'ları arama terimlerine göre sorgulanabilir şekilde filtreler
        /// </summary>
        /// <param name="searchTerm">Arama terimi</param>
        /// <returns>Sorgulanabilir filtrelenmiş silinmiş laptop listesi</returns>
        IQueryable<Laptop> SearchDeletedLaptopsQueryable(string searchTerm);

        /// <summary>
        /// Laptop verilerini Excel formatında export eder
        /// </summary>
        /// <returns>Excel dosyası byte array'i</returns>
        Task<byte[]> ExportLaptopsToExcelAsync();

        /// <summary>
        /// Laptop verilerini CSV formatında export eder
        /// </summary>
        /// <returns>CSV dosyası byte array'i</returns>
        Task<byte[]> ExportLaptopsToCsvAsync();

        /// <summary>
        /// Upload edilen dosyadan laptop verilerini import eder
        /// </summary>
        /// <param name="fileBytes">Dosya byte array'i</param>
        /// <param name="fileName">Dosya adı</param>
        /// <returns>Import işlem sonucu</returns>
        Task<(bool Success, string Message, int ImportedCount)> ImportLaptopsFromFileAsync(byte[] fileBytes, string fileName);
    }
}
