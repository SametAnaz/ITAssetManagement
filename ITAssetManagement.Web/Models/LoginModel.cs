using System.ComponentModel.DataAnnotations;

namespace ITAssetManagement.Web.Models
{
    /// <summary>
    /// Kullanıcı girişi için kullanılan model sınıfı.
    /// Bu sınıf, kullanıcı kimlik doğrulama sürecinde form verilerini taşır.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Bu model şu işlemlerde kullanılır:
    /// <list type="bullet">
    /// <item><description>Kullanıcı girişi formu</description></item>
    /// <item><description>API authentication</description></item>
    /// <item><description>Oturum yönetimi</description></item>
    /// </list>
    /// </para>
    /// <para>
    /// Güvenlik özellikleri:
    /// <list type="bullet">
    /// <item><description>Password alanı DataType.Password ile işaretlenmiştir</description></item>
    /// <item><description>Required attribute'ları ile boş giriş engellenir</description></item>
    /// <item><description>Client ve server tarafında validasyon yapılır</description></item>
    /// </list>
    /// </para>
    /// </remarks>
    /// <example>
    /// Login form örneği:
    /// <code>
    /// @model LoginModel
    /// &lt;form method="post"&gt;
    ///     &lt;input asp-for="Username" /&gt;
    ///     &lt;input asp-for="Password" type="password" /&gt;
    ///     &lt;button type="submit"&gt;Giriş&lt;/button&gt;
    /// &lt;/form&gt;
    /// </code>
    /// </example>
    public class LoginModel
    {
        /// <summary>
        /// Kullanıcı adı.
        /// Sistemde kayıtlı benzersiz kullanıcı tanımlayıcısı.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Validasyon kuralları:
        /// <list type="bullet">
        /// <item><description>Boş bırakılamaz</description></item>
        /// <item><description>Büyük/küçük harf duyarlıdır</description></item>
        /// <item><description>Özel karakter ve boşluk içermemelidir</description></item>
        /// </list>
        /// </para>
        /// </remarks>
        [Required(ErrorMessage = "Kullanıcı adı gerekli")]
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// Kullanıcı şifresi.
        /// Güvenli kimlik doğrulama için kullanılır.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Güvenlik özellikleri:
        /// <list type="bullet">
        /// <item><description>UI'da yıldız (*) şeklinde gösterilir</description></item>
        /// <item><description>POST işleminde şifrelenerek gönderilir</description></item>
        /// <item><description>Veritabanında hash'lenerek saklanır</description></item>
        /// </list>
        /// </para>
        /// </remarks>
        [Required(ErrorMessage = "Şifre gerekli")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }
}
