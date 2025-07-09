using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITAssetManagement.Models
{
    public class LaptopLog
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int LaptopId { get; set; }

        [ForeignKey("LaptopId")]
        public virtual Laptop? Laptop { get; set; }

        [Required]
        [StringLength(100)]
        public string Action { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Details { get; set; }

        [Required]
        public DateTime LogDate { get; set; } = DateTime.Now;

        [StringLength(100)]
        public string? UserId { get; set; }
    }
}
