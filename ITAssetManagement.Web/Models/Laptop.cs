using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITAssetManagement.Web.Models
{
    public class Laptop
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Tag Number")]
        public string EtiketNo { get; set; } = string.Empty;

        [NotMapped]
        public string DisplayName => $"{Marka} {Model} ({EtiketNo})";

        [Required]
        [StringLength(100)]
        [Display(Name = "Brand")]
        public string Marka { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        [Display(Name = "Model")]
        public string Model { get; set; } = string.Empty;

        [StringLength(1000)]
        [Display(Name = "Specifications")]
        public string? Ozellikler { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Status")]
        public string Durum { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Registration Date")]
        [DataType(DataType.DateTime)]
        public DateTime KayitTarihi { get; set; } = DateTime.Now;

        [Required]
        [Display(Name = "Is Active")]
        public bool IsActive { get; set; } = true;

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
