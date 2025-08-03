using ITAssetManagement.Web.Services.Interfaces;
using ITAssetManagement.Web.Models;
using ITAssetManagement.Web.Data;
using ITAssetManagement.Web.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ITAssetManagement.Web.Services
{
    /// <summary>
    /// Laptop varlıklarını yöneten servis sınıfı.
    /// ILaptopService arayüzünü implement eder.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Bu servis şu işlevleri sağlar:
    /// <list type="bullet">
    /// <item><description>Laptop envanteri yönetimi</description></item>
    /// <item><description>Zimmet durumu takibi</description></item>
    /// <item><description>Arama ve filtreleme</description></item>
    /// <item><description>Silinen laptopların yönetimi</description></item>
    /// </list>
    /// </para>
    /// </remarks>
    public class LaptopService : ILaptopService
    {
        private readonly ILaptopRepository _laptopRepository;
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// LaptopService sınıfının yeni bir instance'ını oluşturur.
        /// </summary>
        /// <param name="laptopRepository">Laptop repository'si</param>
        /// <param name="context">Veritabanı context'i</param>
        /// <remarks>
        /// Dependency injection ile gerekli bağımlılıkları alır.
        /// </remarks>
        public LaptopService(ILaptopRepository laptopRepository, ApplicationDbContext context)
        {
            _laptopRepository = laptopRepository;
            _context = context;
        }

        /// <summary>
        /// Tüm aktif laptopları sorgulanabilir şekilde getirir.
        /// </summary>
        /// <returns>Sorgulanabilir laptop listesi</returns>
        /// <remarks>
        /// Include edilmiş ilişkiler:
        /// - CurrentAssignment
        /// - User (through CurrentAssignment)
        /// </remarks>
        public IQueryable<Laptop> GetAllLaptopsQueryable()
        {
            return _context.Laptops
                .Include(l => l.CurrentAssignment)
                .ThenInclude(a => a!.User)
                .Where(l => l.IsActive);
        }

        /// <summary>
        /// Tüm aktif laptopları listeler.
        /// </summary>
        /// <returns>Laptop listesi</returns>
        /// <remarks>
        /// GetAllLaptopsQueryable metodunun execute edilmiş hali.
        /// </remarks>
        public async Task<IEnumerable<Laptop>> GetAllLaptopsAsync()
        {
            return await GetAllLaptopsQueryable().ToListAsync();
        }

        /// <summary>
        /// Zimmetlenmemiş laptopları sorgulanabilir şekilde getirir.
        /// </summary>
        /// <returns>Sorgulanabilir müsait laptop listesi</returns>
        /// <remarks>
        /// Şu koşulları sağlayan laptopları getirir:
        /// - IsActive = true
        /// - Aktif zimmeti olmayan
        /// </remarks>
        public IQueryable<Laptop> GetAvailableLaptopsQueryable()
        {
            var assignedLaptopIds = _context.Assignments
                .Where(a => a.ReturnDate == null)
                .Select(a => a.LaptopId);

            return _context.Laptops
                .Where(l => l.IsActive && !assignedLaptopIds.Contains(l.Id));
        }

        /// <summary>
        /// Zimmetlenmemiş laptopları listeler.
        /// </summary>
        /// <returns>Müsait laptop listesi</returns>
        /// <remarks>
        /// GetAvailableLaptopsQueryable metodunun execute edilmiş hali.
        /// </remarks>
        public async Task<IEnumerable<Laptop>> GetAvailableLaptopsAsync()
        {
            return await GetAvailableLaptopsQueryable().ToListAsync();
        }

        /// <summary>
        /// ID'ye göre laptop getirir.
        /// </summary>
        /// <param name="id">Laptop ID'si</param>
        /// <returns>Bulunan laptop veya null</returns>
        /// <remarks>
        /// Temel laptop bilgilerini içerir, ilişkili veriler include edilmez.
        /// </remarks>
        public async Task<Laptop?> GetLaptopByIdAsync(int id)
        {
            return await _laptopRepository.GetByIdAsync(id);
        }

        /// <summary>
        /// ID'ye göre laptop'u tüm detaylarıyla getirir.
        /// </summary>
        /// <param name="id">Laptop ID'si</param>
        /// <returns>Detaylı laptop bilgisi veya null</returns>
        /// <remarks>
        /// Include edilen ilişkiler:
        /// - Photos
        /// - Loglar
        /// - CurrentAssignment
        /// - User (through CurrentAssignment)
        /// </remarks>
        public async Task<Laptop?> GetLaptopWithDetailsAsync(int id)
        {
            return await _laptopRepository.GetLaptopWithDetailsAsync(id);
        }

        /// <summary>
        /// Yeni laptop kaydı oluşturur.
        /// </summary>
        /// <param name="laptop">Oluşturulacak laptop bilgileri</param>
        /// <returns>İşlem başarılı ise true</returns>
        /// <remarks>
        /// <para>
        /// Bu metot şu işlemleri gerçekleştirir:
        /// <list type="bullet">
        /// <item><description>Veri doğrulama</description></item>
        /// <item><description>Yeni laptop kaydı</description></item>
        /// <item><description>İlişkili koleksiyonların başlatılması</description></item>
        /// </list>
        /// </para>
        /// </remarks>
        public async Task<bool> CreateLaptopAsync(Laptop laptop)
        {
            if (laptop == null)
                return false;

            await _laptopRepository.AddAsync(laptop);
            return await _laptopRepository.SaveChangesAsync();
        }

        /// <summary>
        /// Laptop bilgilerini günceller.
        /// </summary>
        /// <param name="laptop">Güncellenecek laptop bilgileri</param>
        /// <returns>İşlem başarılı ise true</returns>
        /// <remarks>
        /// <para>
        /// Bu metot şu işlemleri gerçekleştirir:
        /// <list type="bullet">
        /// <item><description>Veri doğrulama</description></item>
        /// <item><description>İlgili alanların güncellenmesi</description></item>
        /// <item><description>Değişikliklerin kaydedilmesi</description></item>
        /// </list>
        /// </para>
        /// </remarks>
        public async Task<bool> UpdateLaptopAsync(Laptop laptop)
        {
            if (laptop == null)
                return false;

            _laptopRepository.Update(laptop);
            return await _laptopRepository.SaveChangesAsync();
        }

        /// <summary>
        /// Laptop'u soft delete ile siler.
        /// </summary>
        /// <param name="id">Silinecek laptop ID'si</param>
        /// <param name="silmeNedeni">Silme nedeni açıklaması</param>
        /// <returns>İşlem başarılı ise true</returns>
        /// <remarks>
        /// <para>
        /// Bu metot şu işlemleri gerçekleştirir:
        /// <list type="bullet">
        /// <item><description>IsActive = false yapılır</description></item>
        /// <item><description>Silme tarihi ve nedeni kaydedilir</description></item>
        /// <item><description>İşlemi yapan kullanıcı bilgisi tutulur</description></item>
        /// </list>
        /// </para>
        /// </remarks>
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

        /// <summary>
        /// Silinen laptopları sorgulanabilir şekilde getirir.
        /// </summary>
        /// <returns>Sorgulanabilir silinmiş laptop listesi</returns>
        /// <remarks>
        /// IsActive = false olan kayıtları listeler.
        /// </remarks>
        public IQueryable<Laptop> GetDeletedLaptopsQueryable()
        {
            return _context.Laptops
                .Where(l => !l.IsActive);
        }

        /// <summary>
        /// Silinmiş laptopları listeler.
        /// </summary>
        /// <returns>Silinmiş laptop listesi</returns>
        /// <remarks>
        /// GetDeletedLaptopsQueryable metodunun execute edilmiş hali.
        /// </remarks>
        public async Task<IEnumerable<Laptop>> GetDeletedLaptopsAsync()
        {
            return await GetDeletedLaptopsQueryable().ToListAsync();
        }

        /// <summary>
        /// Silinmiş bir laptop'u geri yükler.
        /// </summary>
        /// <param name="id">Geri yüklenecek laptop ID'si</param>
        /// <returns>İşlem başarılı ise true</returns>
        /// <remarks>
        /// <para>
        /// Bu metot şu işlemleri gerçekleştirir:
        /// <list type="bullet">
        /// <item><description>IsActive = true yapılır</description></item>
        /// <item><description>Silme ile ilgili alanlar temizlenir</description></item>
        /// <item><description>Değişiklikler kaydedilir</description></item>
        /// </list>
        /// </para>
        /// </remarks>
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

        /// <summary>
        /// Laptopları arama terimlerine göre sorgulanabilir şekilde filtreler.
        /// </summary>
        /// <param name="searchTerm">Arama terimi</param>
        /// <returns>Sorgulanabilir filtrelenmiş laptop listesi</returns>
        /// <remarks>
        /// <para>
        /// Arama şu alanlarda yapılır:
        /// <list type="bullet">
        /// <item><description>Marka</description></item>
        /// <item><description>Model</description></item>
        /// <item><description>ID</description></item>
        /// <item><description>Etiket No</description></item>
        /// </list>
        /// </para>
        /// </remarks>
        public IQueryable<Laptop> SearchLaptopsQueryable(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return GetAllLaptopsQueryable();

            searchTerm = searchTerm.ToLower();
            return _context.Laptops
                .Include(l => l.CurrentAssignment)
                .ThenInclude(a => a!.User)
                .Where(l => l.IsActive &&
                    (l.Marka.ToLower().Contains(searchTerm) ||
                     l.Model.ToLower().Contains(searchTerm) ||
                     l.Id.ToString() == searchTerm ||
                     l.EtiketNo.ToLower().Contains(searchTerm)));
        }

        /// <summary>
        /// Laptopları arama terimlerine göre filtreler.
        /// </summary>
        /// <param name="searchTerm">Arama terimi</param>
        /// <returns>Filtrelenmiş laptop listesi</returns>
        /// <remarks>
        /// SearchLaptopsQueryable metodunun execute edilmiş hali.
        /// </remarks>
        public async Task<IEnumerable<Laptop>> SearchLaptopsAsync(string searchTerm)
        {
            return await SearchLaptopsQueryable(searchTerm).ToListAsync();
        }
    }
}
