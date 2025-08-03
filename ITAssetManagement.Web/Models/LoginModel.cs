using System.ComponentModel.DataAnnotations;

namespace ITAssetManagement.Web.Models
{
    /// <summary>
    /// Kullanıcı girişi için kullanılan model sınıfı
    /// </summary>
    public class LoginModel
    {
        /// <summary>
        /// Kullanıcı adı
        /// </summary>
        [Required(ErrorMessage = "Kullanıcı adı gerekli")]
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// Kullanıcı şifresi
        /// </summary>
        [Required(ErrorMessage = "Şifre gerekli")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }
}
