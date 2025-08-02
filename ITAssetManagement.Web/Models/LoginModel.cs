using System.ComponentModel.DataAnnotations;

namespace ITAssetManagement.Web.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Kullanıcı adı gerekli")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Şifre gerekli")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
