using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITAssetManagement.Models
{
    public class LaptopPhoto
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int LaptopId { get; set; }

        [ForeignKey("LaptopId")]
        public virtual Laptop? Laptop { get; set; }

        [Required]
        [StringLength(255)]
        [Display(Name = "Path")]
        public string Path { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        [Required]
        [Display(Name = "Upload Date")]
        public DateTime YuklemeTarihi { get; set; } = DateTime.Now;
    }
}