using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITAssetManagement.Web.Models.Email
{
    /// <summary>
    /// Gönderilen e-postaların log kayıtlarını tutan sınıf
    /// </summary>
    public class EmailLog
    {
        /// <summary>
        /// Log kaydının benzersiz kimlik numarası
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Alıcının e-posta adresi
        /// </summary>
        [Required]
        public string ToEmail { get; set; } = string.Empty;

        /// <summary>
        /// E-posta konusu
        /// </summary>
        [Required]
        public string Subject { get; set; } = string.Empty;

        /// <summary>
        /// E-posta içeriği
        /// </summary>
        public string? Body { get; set; }

        /// <summary>
        /// E-postanın gönderildiği tarih
        /// </summary>
        [Required]
        public DateTime SentDate { get; set; } = DateTime.Now;

        /// <summary>
        /// E-posta gönderiminin başarılı olup olmadığı
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// Hata durumunda hata mesajı
        /// </summary>
        public string? ErrorMessage { get; set; }

        /// <summary>
        /// E-postanın ilişkili olduğu entity tipi (Zimmet, Laptop vb.)
        /// </summary>
        public string? RelatedEntityType { get; set; }

        /// <summary>
        /// İlişkili entity'nin ID'si
        /// </summary>
        public int? RelatedEntityId { get; set; }

        /// <summary>
        /// Başarısız gönderim durumunda deneme sayısı
        /// </summary>
        public int? RetryCount { get; set; }
    }
}