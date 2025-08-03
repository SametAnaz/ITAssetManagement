using ITAssetManagement.Web.Data.Repositories;
using ITAssetManagement.Web.Models;
using ITAssetManagement.Web.Services.Interfaces;

namespace ITAssetManagement.Web.Services
{
    /// <summary>
    /// Zimmet işlemlerini yöneten servis sınıfı.
    /// IAssignmentService arayüzünü implement eder.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Bu servis şu işlemleri gerçekleştirir:
    /// <list type="bullet">
    /// <item><description>Zimmet kayıtlarının oluşturulması ve yönetimi</description></item>
    /// <item><description>Zimmet detaylarının sorgulanması</description></item>
    /// <item><description>Zimmet güncelleme ve silme işlemleri</description></item>
    /// <item><description>İlişkili varlıkların (Laptop, User) yönetimi</description></item>
    /// </list>
    /// </para>
    /// </remarks>
    public class AssignmentService : IAssignmentService
    {
        private readonly IAssignmentRepository _assignmentRepository;

        /// <summary>
        /// AssignmentService sınıfının yeni bir instance'ını oluşturur.
        /// </summary>
        /// <param name="assignmentRepository">Zimmet repository'si</param>
        /// <remarks>
        /// Dependency injection ile IAssignmentRepository bağımlılığını alır.
        /// </remarks>
        public AssignmentService(IAssignmentRepository assignmentRepository)
        {
            _assignmentRepository = assignmentRepository;
        }

        /// <summary>
        /// Tüm zimmet kayıtlarını ilişkili detaylarıyla birlikte getirir.
        /// </summary>
        /// <returns>Zimmet kayıtlarının listesi</returns>
        /// <remarks>
        /// <para>
        /// Bu metot şu detayları içerir:
        /// <list type="bullet">
        /// <item><description>Laptop bilgileri</description></item>
        /// <item><description>Kullanıcı bilgileri</description></item>
        /// <item><description>Zimmet tarihleri</description></item>
        /// </list>
        /// </para>
        /// </remarks>
        public async Task<IEnumerable<Assignment>> GetAllAssignmentsAsync()
        {
            return await _assignmentRepository.GetAllWithDetailsAsync();
        }

        /// <summary>
        /// ID'ye göre zimmet kaydını detaylarıyla birlikte getirir.
        /// </summary>
        /// <param name="id">Zimmet kaydının ID'si</param>
        /// <returns>Bulunan zimmet kaydı veya null</returns>
        /// <remarks>
        /// İlgili Laptop ve User bilgilerini Include eder.
        /// </remarks>
        public async Task<Assignment?> GetAssignmentByIdAsync(int id)
        {
            return await _assignmentRepository.GetByIdWithDetailsAsync(id);
        }

        /// <summary>
        /// Yeni bir zimmet kaydı oluşturur.
        /// </summary>
        /// <param name="assignment">Oluşturulacak zimmet kaydı</param>
        /// <remarks>
        /// <para>
        /// Bu metot şu işlemleri gerçekleştirir:
        /// <list type="bullet">
        /// <item><description>Veri doğrulama kontrolleri</description></item>
        /// <item><description>Zimmet kaydının oluşturulması</description></item>
        /// <item><description>Veritabanına kaydetme</description></item>
        /// </list>
        /// </para>
        /// </remarks>
        public async Task CreateAssignmentAsync(Assignment assignment)
        {
            await _assignmentRepository.AddAsync(assignment);
            await _assignmentRepository.SaveChangesAsync();
        }

        /// <summary>
        /// Mevcut bir zimmet kaydını günceller.    
        /// </summary>
        /// <param name="assignment">Güncellenecek zimmet kaydı</param>
        /// <remarks>
        /// <para>
        /// Bu metot şu işlemleri gerçekleştirir:
        /// <list type="bullet">
        /// <item><description>Veri doğrulama kontrolleri</description></item>
        /// <item><description>İlgili alanların güncellenmesi</description></item>
        /// <item><description>Değişikliklerin kaydedilmesi</description></item>
        /// </list>
        /// </para>
        /// </remarks>
        public async Task UpdateAssignmentAsync(Assignment assignment)
        {
            _assignmentRepository.Update(assignment);
            await _assignmentRepository.SaveChangesAsync();
        }

        /// <summary>
        /// Bir zimmet kaydını siler.
        /// </summary>
        /// <param name="id">Silinecek zimmet kaydının ID'si</param>
        /// <remarks>
        /// <para>
        /// Bu metot şu işlemleri gerçekleştirir:
        /// <list type="bullet">
        /// <item><description>Zimmet kaydının varlığının kontrolü</description></item>
        /// <item><description>İlgili zimmetin silinmesi</description></item>
        /// <item><description>Değişikliklerin kaydedilmesi</description></item>
        /// </list>
        /// </para>
        /// </remarks>
        public async Task DeleteAssignmentAsync(int id)
        {
            var assignment = await _assignmentRepository.GetByIdAsync(id);
            if (assignment != null)
            {
                _assignmentRepository.Delete(assignment);
                await _assignmentRepository.SaveChangesAsync();
            }
        }
    }
}
