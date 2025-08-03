using System.ComponentModel.DataAnnotations;

namespace ITAssetManagement.Web.Models.Email
{
    /// <summary>
    /// E-posta gönderimi için SMTP yapılandırma sınıfı
    /// </summary>
    public class EmailConfiguration
    {
        /// <summary>
        /// SMTP sunucu adresi
        /// </summary>
        [Required]
        public string SmtpHost { get; set; } = string.Empty;

        /// <summary>
        /// SMTP sunucu portu
        /// </summary>
        [Required]
        public int SmtpPort { get; set; }

        /// <summary>
        /// SMTP sunucu kullanıcı adı
        /// </summary>
        [Required]
        public string SmtpUsername { get; set; } = string.Empty;

        /// <summary>
        /// SMTP sunucu şifresi
        /// </summary>
        [Required]
        public string SmtpPassword { get; set; } = string.Empty;

        /// <summary>
        /// SSL kullanılıp kullanılmayacağı
        /// </summary>
        [Required]
        public bool EnableSsl { get; set; }

        /// <summary>
        /// Gönderen e-posta adresi
        /// </summary>
        [Required]
        [EmailAddress]
        public string FromEmail { get; set; } = string.Empty;

        /// <summary>
        /// Gönderen adı
        /// </summary>
        [Required]
        public string FromName { get; set; } = string.Empty;
    }
}