using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITAssetManagement.Web.Models
{
    /// <summary>
    /// Laptop'lara ait fotoğrafları temsil eden sınıf
    /// </summary>
    public class LaptopPhoto
    {
        /// <summary>
        /// Fotoğrafın benzersiz kimlik numarası
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Fotoğrafın ait olduğu laptop'un ID'si
        /// </summary>
        [Required]
        public int LaptopId { get; set; }

        /// <summary>
        /// Fotoğrafın ait olduğu laptop nesnesi
        /// </summary>
        [ForeignKey("LaptopId")]
        public virtual Laptop? Laptop { get; set; }

        /// <summary>
        /// Fotoğrafın dosya sistemi üzerindeki yolu
        /// </summary>
        [Required]
        [StringLength(255)]
        [Display(Name = "Path")]
        public string Path { get; set; } = string.Empty;

        /// <summary>
        /// Fotoğraf için açıklama metni
        /// </summary>
        [StringLength(500)]
        public string? Description { get; set; }

        /// <summary>
        /// Fotoğrafın sisteme yüklenme tarihi
        /// </summary>
        [Required]
        [Display(Name = "Upload Date")]
        public DateTime YuklemeTarihi { get; set; } = DateTime.Now;
    }
}
