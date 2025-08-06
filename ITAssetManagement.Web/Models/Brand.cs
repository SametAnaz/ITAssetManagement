using System.ComponentModel.DataAnnotations;

namespace ITAssetManagement.Web.Models
{
    /// <summary>
    /// Laptop markalarını temsil eden sınıf.
    /// Bu sınıf, sistemde bulunan tüm laptop markalarının yönetimini sağlar.
    /// </summary>
    public class Brand
    {
        /// <summary>
        /// Markanın benzersiz kimlik numarası
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Marka adı
        /// </summary>
        [Required]
        [StringLength(100)]
        [Display(Name = "Marka Adı")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Markanın aktif olup olmadığını belirten flag
        /// </summary>
        [Required]
        [Display(Name = "Aktif")]
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Markanın sisteme kayıt tarihi
        /// </summary>
        [Required]
        [Display(Name = "Kayıt Tarihi")]
        [DataType(DataType.DateTime)]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        /// <summary>
        /// Bu markaya ait laptoplar
        /// </summary>
        public virtual ICollection<Laptop> Laptops { get; set; } = new List<Laptop>();
    }
}
