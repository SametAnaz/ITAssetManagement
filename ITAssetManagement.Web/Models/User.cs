using System.ComponentModel.DataAnnotations;

namespace ITAssetManagement.Web.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Full Name")]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [StringLength(20)]
        [Display(Name = "Phone")]
        public string? Phone { get; set; }

        [StringLength(100)]
        [Display(Name = "Department")]
        public string? Department { get; set; }

        [StringLength(100)]
        [Display(Name = "Position")]
        public string? Position { get; set; }
    }
}
