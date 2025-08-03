using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITAssetManagement.Web.Models
{
    /// <summary>
    /// Sistemdeki laptop varlıklarını temsil eden sınıf
    /// </summary>
    public class Laptop
    {
        /// <summary>
        /// Laptopun benzersiz kimlik numarası
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Laptopun etiket numarası
        /// </summary>
        [Required]
        [StringLength(50)]
        [Display(Name = "Tag Number")]
        public string EtiketNo { get; set; } = string.Empty;

        /// <summary>
        /// Laptopun görüntülenen adı (Marka Model (EtiketNo))
        /// </summary>
        [NotMapped]
        public string DisplayName => $"{Marka} {Model} ({EtiketNo})";

        /// <summary>
        /// Laptopun markası
        /// </summary>
        [Required]
        [StringLength(100)]
        [Display(Name = "Brand")]
        public string Marka { get; set; } = string.Empty;

        /// <summary>
        /// Laptopun modeli
        /// </summary>
        [Required]
        [StringLength(100)]
        [Display(Name = "Model")]
        public string Model { get; set; } = string.Empty;

        /// <summary>
        /// Laptopun teknik özellikleri
        /// </summary>
        [StringLength(1000)]
        [Display(Name = "Specifications")]
        public string? Ozellikler { get; set; }

        /// <summary>
        /// Laptopun mevcut durumu (Aktif, Bakımda, Hurda vb.)
        /// </summary>
        [Required]
        [StringLength(50)]
        [Display(Name = "Status")]
        public string Durum { get; set; } = string.Empty;

        /// <summary>
        /// Laptopun sisteme kayıt tarihi
        /// </summary>
        [Required]
        [Display(Name = "Registration Date")]
        [DataType(DataType.DateTime)]
        public DateTime KayitTarihi { get; set; } = DateTime.Now;

        /// <summary>
        /// Laptopun aktif olup olmadığını belirten flag
        /// </summary>
        [Required]
        [Display(Name = "Is Active")]
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Laptop silindiğinde belirtilen silme nedeni
        /// </summary>
        [Display(Name = "Deletion Reason")]
        [StringLength(500)]
        public string? SilmeNedeni { get; set; }

        [Display(Name = "Deletion Date")]
        [DataType(DataType.DateTime)]
        public DateTime? SilinmeTarihi { get; set; }

        [Display(Name = "Deleted By")]
        [StringLength(100)]
        public string? SilenKullanici { get; set; }

        [Display(Name = "Notes")]
        [StringLength(1000)]
        [DataType(DataType.MultilineText)]
        public string? Notes { get; set; }

        // Navigation properties for related entities
        [Display(Name = "Photos")]
        public virtual ICollection<LaptopPhoto>? Photos { get; set; }

        [Display(Name = "Logs")]
        public virtual ICollection<LaptopLog>? Loglar { get; set; }

        [Display(Name = "Current Assignment")]
        public virtual Assignment? CurrentAssignment { get; set; }

        // Constructor to initialize collections
        public Laptop()
        {
            Photos = new List<LaptopPhoto>();
            Loglar = new List<LaptopLog>();
        }
    }
}
