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
        [Display(Name = "User")]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User? User { get; set; }

        [Required]
        [Display(Name = "Assignment Date")]
        public DateTime AssignmentDate { get; set; } = DateTime.Now;

        [Display(Name = "Return Date")]
        public DateTime? ReturnDate { get; set; }

        [Required]
        [Display(Name = "Date")]
        [DataType(DataType.DateTime)]
        public DateTime Tarih { get; set; } = DateTime.Now;

        [Required]
        [StringLength(50)]
        [Display(Name = "Operation Type")]
        public string IslemTipi { get; set; } = string.Empty; // "Teslim", "İade", etc.
    }
}
