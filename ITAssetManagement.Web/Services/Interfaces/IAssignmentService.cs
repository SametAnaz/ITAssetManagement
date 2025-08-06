using ITAssetManagement.Web.Models;

namespace ITAssetManagement.Web.Services.Interfaces
{
    /// <summary>
    /// Zimmet işlemlerini yöneten servis arayüzü.
    /// Bu servis, laptop zimmet atama, güncelleme ve silme işlemlerini yönetir.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Bu servis aşağıdaki temel işlevleri sağlar:
    /// <list type="bullet">
    /// <item><description>Zimmet kayıtlarının listelenmesi ve filtrelenmesi</description></item>
    /// <item><description>Yeni zimmet oluşturma ve güncelleme</description></item>
    /// <item><description>Zimmet iade işlemleri</description></item>
    /// <item><description>Zimmet geçmişi takibi</description></item>
    /// </list>
    /// </para>
    /// </remarks>
    public interface IAssignmentService
    {
        /// <summary>
        /// Tüm zimmet kayıtlarını sorgulanabilir şekilde getirir.
        /// </summary>
        /// <returns>Sorgulanabilir zimmet listesi</returns>
        /// <remarks>
        /// Bu metot aktif ve geçmiş tüm zimmetleri içerir.
        /// Sonuçlar tarihe göre azalan sırada döner.
        /// </remarks>
        IQueryable<Assignment> GetAllAssignmentsQueryable();

        /// <summary>
        /// Belirtilen arama terimine göre zimmet kayıtlarını filtreleyerek getirir.
        /// </summary>
        /// <param name="searchTerm">Arama terimi</param>
        /// <returns>Filtrelenmiş zimmet listesi</returns>
        IQueryable<Assignment> SearchAssignmentsQueryable(string searchTerm);

        /// <summary>
        /// Belirtilen ID'ye sahip zimmet kaydını getirir.
        /// </summary>
        /// <param name="id">Zimmet kaydının ID'si</param>
        /// <returns>Bulunan zimmet kaydı veya null</returns>
        /// <remarks>
        /// Bu metot, ilişkili Laptop ve User bilgilerini de include eder.
        /// </remarks>
        Task<Assignment?> GetAssignmentByIdAsync(int id);

        /// <summary>
        /// Yeni bir zimmet kaydı oluşturur.
        /// </summary>
        /// <param name="assignment">Oluşturulacak zimmet kaydı</param>
        /// <returns>Oluşturma işlemi tamamlandığında dönen task</returns>
        /// <remarks>
        /// <para>
        /// Bu metot şu işlemleri gerçekleştirir:
        /// <list type="bullet">
        /// <item><description>Laptop durumunu günceller</description></item>
        /// <item><description>Zimmet kaydını oluşturur</description></item>
        /// <item><description>İlgili log kayıtlarını oluşturur</description></item>
        /// </list>
        /// </para>
        /// </remarks>
        Task CreateAssignmentAsync(Assignment assignment);

        /// <summary>
        /// Mevcut bir zimmet kaydını günceller.
        /// </summary>
        /// <param name="assignment">Güncellenecek zimmet kaydı</param>
        /// <returns>Güncelleme işlemi tamamlandığında dönen task</returns>
        /// <remarks>
        /// <para>
        /// Bu metot şu durumları yönetir:
        /// <list type="bullet">
        /// <item><description>Zimmet süre uzatma</description></item>
        /// <item><description>İade tarihi güncelleme</description></item>
        /// <item><description>Notlar ve detay güncelleme</description></item>
        /// </list>
        /// </para>
        /// </remarks>
        Task UpdateAssignmentAsync(Assignment assignment);

        /// <summary>
        /// Bir zimmet kaydını siler.
        /// </summary>
        /// <param name="id">Silinecek zimmet kaydının ID'si</param>
        /// <returns>Silme işlemi tamamlandığında dönen task</returns>
        /// <remarks>
        /// <para>
        /// Bu metot şu işlemleri gerçekleştirir:
        /// <list type="bullet">
        /// <item><description>İlgili laptop durumunu günceller</description></item>
        /// <item><description>Zimmet kaydını arşivler</description></item>
        /// <item><description>İlgili log kayıtlarını oluşturur</description></item>
        /// </list>
        /// </para>
        /// </remarks>
        Task DeleteAssignmentAsync(int id);

        /// <summary>
        /// Zimmet verilerini Excel formatında export eder
        /// </summary>
        /// <returns>Excel dosyası byte array'i</returns>
        Task<byte[]> ExportAssignmentsToExcelAsync();

        /// <summary>
        /// Zimmet verilerini CSV formatında export eder
        /// </summary>
        /// <returns>CSV dosyası byte array'i</returns>
        Task<byte[]> ExportAssignmentsToCsvAsync();

        /// <summary>
        /// Upload edilen dosyadan zimmet verilerini import eder
        /// </summary>
        /// <param name="fileBytes">Dosya byte array'i</param>
        /// <param name="fileName">Dosya adı</param>
        /// <returns>Import işlem sonucu</returns>
        Task<(bool Success, string Message, int ImportedCount)> ImportAssignmentsFromFileAsync(byte[] fileBytes, string fileName);
    }
}
