using System.ComponentModel.DataAnnotations;

namespace ITAssetManagement.Web.Models
{
    /// <summary>
    /// Sistemdeki kullanıcıları temsil eden sınıf
    /// </summary>
    public class User
    {
        /// <summary>
        /// Kullanıcının benzersiz kimlik numarası
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Kullanıcının tam adı
        /// </summary>
        [Required]
        [StringLength(100)]
        [Display(Name = "Full Name")]
        public string FullName { get; set; } = string.Empty;

        /// <summary>
        /// Kullanıcının email adresi
        /// </summary>
        [Required]
        [StringLength(100)]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Kullanıcının telefon numarası
        /// </summary>
        [StringLength(20)]
        [Display(Name = "Phone")]
        public string? Phone { get; set; }

        /// <summary>
        /// Kullanıcının çalıştığı departman
        /// </summary>
        [StringLength(100)]
        [Display(Name = "Department")]
        public string? Department { get; set; }

        /// <summary>
        /// Kullanıcının pozisyonu
        /// </summary>
        [StringLength(100)]
        [Display(Name = "Position")]
        public string? Position { get; set; }

        /// <summary>
        /// Kullanıcıya zimmetlenmiş varlıkların listesi
        /// </summary>
        public List<Assignment> Assignments { get; set; } = new List<Assignment>();
    }
}
