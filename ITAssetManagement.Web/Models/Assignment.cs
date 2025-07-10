using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITAssetManagement.Web.Models
{
    public class Assignment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int LaptopId { get; set; }

        [ForeignKey("LaptopId")]
        public virtual Laptop? Laptop { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "User ID")]
        public string UserId { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Date")]
        [DataType(DataType.DateTime)]
        public DateTime Tarih { get; set; } = DateTime.Now;

        [Required]
        [StringLength(50)]
        [Display(Name = "Operation Type")]
        public string IslemTipi { get; set; } = string.Empty; // "Teslim", "İade", etc.

        [StringLength(1000)]
        [Display(Name = "Description")]
        public string? Aciklama { get; set; }

        [StringLength(255)]
        [Display(Name = "Signature Path")]
        public string? ImzaYolu { get; set; }
    }
}
