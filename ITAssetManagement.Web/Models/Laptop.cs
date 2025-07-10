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

        // Navigation properties for related entities
        [Display(Name = "Photos")]
        public virtual ICollection<LaptopPhoto>? Photos { get; set; }

        [Display(Name = "Logs")]
        public virtual ICollection<LaptopLog>? Loglar { get; set; }

        // Constructor to initialize collections
        public Laptop()
        {
            Photos = new List<LaptopPhoto>();
            Loglar = new List<LaptopLog>();
        }
    }
}
