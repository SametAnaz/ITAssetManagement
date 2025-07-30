using System.ComponentModel.DataAnnotations;

namespace ITAssetManagement.Web.Models.Email
{
    public class EmailConfiguration
    {
        [Required]
        public string SmtpHost { get; set; } = string.Empty;

        [Required]
        public int SmtpPort { get; set; }

        [Required]
        public string SmtpUsername { get; set; } = string.Empty;

        [Required]
        public string SmtpPassword { get; set; } = string.Empty;

        [Required]
        public bool EnableSsl { get; set; }

        [Required]
        [EmailAddress]
        public string FromEmail { get; set; } = string.Empty;

        [Required]
        public string FromName { get; set; } = string.Empty;
    }
}