using ITAssetManagement.Web.Models;

namespace ITAssetManagement.Web.Services.Interfaces
{
    /// <summary>
    /// Kullanıcı işlemlerini yöneten servis arayüzü.
    /// Bu servis, kullanıcı yönetimi ve zimmet sahiplerinin bilgilerini yönetir.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Bu servis aşağıdaki temel işlevleri sağlar:
    /// <list type="bullet">
    /// <item><description>Kullanıcı kaydı ve profil yönetimi</description></item>
    /// <item><description>Departman bazlı kullanıcı organizasyonu</description></item>
    /// <item><description>Kullanıcı arama ve filtreleme</description></item>
    /// <item><description>Zimmet sahiplerinin takibi</description></item>
    /// </list>
    /// </para>
    /// </remarks>
    public interface IUserService
    {
        /// <summary>
        /// Tüm kullanıcıları getirir.
        /// </summary>
        /// <returns>Kullanıcı listesi</returns>
        /// <remarks>
        /// Varsayılan olarak aktif kullanıcıları alfabetik sırada getirir.
        /// </remarks>
        Task<IEnumerable<User>> GetAllUsersAsync();

        /// <summary>
        /// ID'ye göre kullanıcı getirir.
        /// </summary>
        /// <param name="id">Kullanıcı ID'si</param>
        /// <returns>Bulunan kullanıcı veya null</returns>
        /// <remarks>
        /// Kullanıcı ile birlikte aktif zimmetlerini de getirir.
        /// </remarks>
        Task<User?> GetUserByIdAsync(int id);

        /// <summary>
        /// Email adresine göre kullanıcı getirir.
        /// </summary>
        /// <param name="email">Kullanıcı email adresi</param>
        /// <returns>Bulunan kullanıcı veya null</returns>
        /// <remarks>
        /// Email adresleri benzersiz olmalıdır.
        /// </remarks>
        Task<User?> GetUserByEmailAsync(string email);

        /// <summary>
        /// Belirli bir departmandaki tüm kullanıcıları getirir.
        /// </summary>
        /// <param name="department">Departman adı</param>
        /// <returns>Departmandaki kullanıcı listesi</returns>
        /// <remarks>
        /// Sonuçlar pozisyona göre sıralanır.
        /// </remarks>
        Task<IEnumerable<User>> GetUsersByDepartmentAsync(string department);

        /// <summary>
        /// Yeni bir kullanıcı oluşturur.
        /// </summary>
        /// <param name="user">Oluşturulacak kullanıcı bilgileri</param>
        /// <returns>İşlem başarılı ise true</returns>
        /// <remarks>
        /// <para>
        /// Oluşturma sırasında:
        /// <list type="bullet">
        /// <item><description>Email benzersizliği kontrol edilir</description></item>
        /// <item><description>Gerekli validasyonlar yapılır</description></item>
        /// <item><description>Hoşgeldin emaili gönderilir</description></item>
        /// </list>
        /// </para>
        /// </remarks>
        Task<bool> CreateUserAsync(User user);

        /// <summary>
        /// Kullanıcı bilgilerini günceller.
        /// </summary>
        /// <param name="user">Güncellenecek kullanıcı bilgileri</param>
        /// <returns>İşlem başarılı ise true</returns>
        /// <remarks>
        /// <para>
        /// Güncelleme sırasında:
        /// <list type="bullet">
        /// <item><description>Email değişikliğinde benzersizlik kontrolü yapılır</description></item>
        /// <item><description>Departman değişikliğinde zimmetler kontrol edilir</description></item>
        /// <item><description>İlgili bildirimler gönderilir</description></item>
        /// </list>
        /// </para>
        /// </remarks>
        Task<bool> UpdateUserAsync(User user);

        /// <summary>
        /// Kullanıcıyı siler.
        /// </summary>
        /// <param name="id">Silinecek kullanıcı ID'si</param>
        /// <returns>İşlem başarılı ise true</returns>
        /// <remarks>
        /// <para>
        /// Silme işlemi öncesinde:
        /// <list type="bullet">
        /// <item><description>Aktif zimmetler kontrol edilir</description></item>
        /// <item><description>Soft delete uygulanır</description></item>
        /// <item><description>İlgili kayıtlar arşivlenir</description></item>
        /// </list>
        /// </para>
        /// </remarks>
        Task<bool> DeleteUserAsync(int id);

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
        /// </list>
        /// </para>
        /// </remarks>
        IQueryable<User> SearchUsersQueryable(string searchTerm);

        /// <summary>
        /// Kullanıcıları arama terimlerine göre filtreler.
        /// </summary>
        /// <param name="searchTerm">Arama terimi</param>
        /// <returns>Filtrelenmiş kullanıcı listesi</returns>
        /// <remarks>
        /// SearchUsersQueryable metodunun execute edilmiş halidir.
        /// </remarks>
        Task<IEnumerable<User>> SearchUsersAsync(string searchTerm);
    }
}
