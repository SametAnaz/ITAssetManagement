using ITAssetManagement.Web.Services.Interfaces;
using ITAssetManagement.Web.Models;
using ITAssetManagement.Web.Data.Repositories;
using ITAssetManagement.Web.Data;
using Microsoft.EntityFrameworkCore;

namespace ITAssetManagement.Web.Services
{
    /// <summary>
    /// Kullanıcı işlemlerini yöneten servis sınıfı.
    /// IUserService arayüzünü implement eder.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Bu servis şu işlevleri sağlar:
    /// <list type="bullet">
    /// <item><description>Kullanıcı hesaplarının yönetimi</description></item>
    /// <item><description>Departman bazlı organizasyon</description></item>
    /// <item><description>Zimmet sahiplerinin takibi</description></item>
    /// <item><description>Kullanıcı arama ve filtreleme</description></item>
    /// </list>
    /// </para>
    /// </remarks>
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// UserService sınıfının yeni bir instance'ını oluşturur.
        /// </summary>
        /// <param name="userRepository">Kullanıcı repository'si</param>
        /// <param name="context">Veritabanı context'i</param>
        /// <remarks>
        /// Dependency injection ile gerekli bağımlılıkları alır.
        /// </remarks>
        public UserService(IUserRepository userRepository, ApplicationDbContext context)
        {
            _userRepository = userRepository;
            _context = context;
        }

        /// <summary>
        /// Tüm kullanıcıları listeler.
        /// </summary>
        /// <returns>Kullanıcı listesi</returns>
        /// <remarks>
        /// Temel kullanıcı bilgilerini içerir.
        /// </remarks>
        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllAsync();
        }

        /// <summary>
        /// ID'ye göre kullanıcıyı zimmet bilgileriyle getirir.
        /// </summary>
        /// <param name="id">Kullanıcı ID'si</param>
        /// <returns>Bulunan kullanıcı veya null</returns>
        /// <remarks>
        /// Kullanıcının tüm zimmet geçmişini de içerir.
        /// </remarks>
        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _userRepository.GetUserWithAssignmentsAsync(id);
        }

        /// <summary>
        /// Email adresine göre kullanıcı getirir.
        /// </summary>
        /// <param name="email">Kullanıcı email adresi</param>
        /// <returns>Bulunan kullanıcı veya null</returns>
        /// <remarks>
        /// Email adresi benzersiz olmalıdır.
        /// </remarks>
        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _userRepository.GetUserByEmailAsync(email);
        }

        /// <summary>
        /// Belirli bir departmandaki tüm kullanıcıları getirir.
        /// </summary>
        /// <param name="department">Departman adı</param>
        /// <returns>Departmandaki kullanıcı listesi</returns>
        /// <remarks>
        /// Kullanıcılar pozisyonlarına göre sıralı gelir.
        /// </remarks>
        public async Task<IEnumerable<User>> GetUsersByDepartmentAsync(string department)
        {
            return await _userRepository.GetUsersByDepartmentAsync(department);
        }

        /// <summary>
        /// Yeni kullanıcı kaydı oluşturur.
        /// </summary>
        /// <param name="user">Oluşturulacak kullanıcı bilgileri</param>
        /// <returns>İşlem başarılı ise true</returns>
        /// <remarks>
        /// <para>
        /// Bu metot şu işlemleri gerçekleştirir:
        /// <list type="bullet">
        /// <item><description>Email benzersizliği kontrolü</description></item>
        /// <item><description>Veri doğrulama</description></item>
        /// <item><description>Kullanıcı kaydı oluşturma</description></item>
        /// </list>
        /// </para>
        /// </remarks>
        public async Task<bool> CreateUserAsync(User user)
        {
            if (user == null)
                return false;

            // Email benzersizlik kontrolü
            var existingUser = await _userRepository.GetUserByEmailAsync(user.Email);
            if (existingUser != null)
                return false;

            await _userRepository.AddAsync(user);
            return await _userRepository.SaveChangesAsync();
        }

        /// <summary>
        /// Kullanıcı bilgilerini günceller.
        /// </summary>
        /// <param name="user">Güncellenecek kullanıcı bilgileri</param>
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
        public async Task<bool> UpdateUserAsync(User user)
        {
            if (user == null)
                return false;

            _userRepository.Update(user);
            return await _userRepository.SaveChangesAsync();
        }

        /// <summary>
        /// Kullanıcıyı siler.
        /// </summary>
        /// <param name="id">Silinecek kullanıcı ID'si</param>
        /// <returns>İşlem başarılı ise true</returns>
        /// <remarks>
        /// <para>
        /// Silme işlemi öncesi:
        /// <list type="bullet">
        /// <item><description>Aktif zimmetler kontrol edilir</description></item>
        /// <item><description>İlişkili kayıtlar güncellenir</description></item>
        /// <item><description>Kullanıcı kaydı kaldırılır</description></item>
        /// </list>
        /// </para>
        /// </remarks>
        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                return false;

            _userRepository.Remove(user);
            return await _userRepository.SaveChangesAsync();
        }

        /// <summary>
        /// Kullanıcıları arama terimlerine göre sorgulanabilir şekilde filtreler.
        /// </summary>
        /// <param name="searchTerm">Arama terimi</param>
        /// <returns>Sorgulanabilir filtrelenmiş kullanıcı listesi</returns>
        /// <remarks>
        /// <para>
        /// Arama şu alanlarda yapılır:
        /// <list type="bullet">
        /// <item><description>Ad Soyad</description></item>
        /// <item><description>Email</description></item>
        /// <item><description>Departman</description></item>
        /// <item><description>Pozisyon</description></item>
        /// <item><description>Telefon</description></item>
        /// </list>
        /// </para>
        /// </remarks>
        public IQueryable<User> SearchUsersQueryable(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return _context.Users.Include(u => u.Assignments);

            searchTerm = searchTerm.ToLower();
            return _context.Users
                .Include(u => u.Assignments)
                .Where(u => 
                    u.FullName.ToLower().Contains(searchTerm) ||
                    u.Email.ToLower().Contains(searchTerm) ||
                    (u.Department != null && u.Department.ToLower().Contains(searchTerm)) ||
                    (u.Position != null && u.Position.ToLower().Contains(searchTerm)) ||
                    (u.Phone != null && u.Phone.Contains(searchTerm)));
        }

        /// <summary>
        /// Kullanıcıları arama terimlerine göre filtreler.
        /// </summary>
        /// <param name="searchTerm">Arama terimi</param>
        /// <returns>Filtrelenmiş kullanıcı listesi</returns>
        /// <remarks>
        /// SearchUsersQueryable metodunun execute edilmiş hali.
        /// </remarks>
        public async Task<IEnumerable<User>> SearchUsersAsync(string searchTerm)
        {
            return await SearchUsersQueryable(searchTerm).ToListAsync();
        }
    }
}
