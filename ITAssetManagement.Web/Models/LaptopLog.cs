using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITAssetManagement.Web.Models
{
    /// <summary>
    /// Laptop üzerinde yapılan işlemlerin loglarını tutan sınıf
    /// </summary>
    public class LaptopLog
    {
        /// <summary>
        /// Log kaydının benzersiz kimlik numarası
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// İşlem yapılan laptop'un ID'si
        /// </summary>
        [Required]
        public int LaptopId { get; set; }

        /// <summary>
        /// İşlem yapılan laptop nesnesi
        /// </summary>
        [ForeignKey("LaptopId")]
        public virtual Laptop? Laptop { get; set; }

        /// <summary>
        /// Yapılan işlemin tipi (Ekleme, Güncelleme, Silme vb.)
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Action { get; set; } = string.Empty;

        /// <summary>
        /// İşlem detayları
        /// </summary>
        [StringLength(1000)]
        public string? Details { get; set; }

        /// <summary>
        /// İşlemin gerçekleştiği tarih
        /// </summary>
        [Required]
        public DateTime LogDate { get; set; } = DateTime.Now;

        /// <summary>
        /// İşlemi yapan kullanıcının ID'si
        /// </summary>
        [StringLength(100)]
        public string? UserId { get; set; }
    }
}
