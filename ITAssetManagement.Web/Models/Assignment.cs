using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITAssetManagement.Web.Models
{
    /// <summary>
    /// Laptop zimmet işlemlerini temsil eden sınıf
    /// </summary>
    public class Assignment
    {
        /// <summary>
        /// Zimmet kaydının benzersiz kimlik numarası
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Zimmetlenen laptopun ID'si
        /// </summary>
        [Required]
        public int LaptopId { get; set; }

        /// <summary>
        /// Zimmetlenen laptop nesnesi
        /// </summary>
        [ForeignKey("LaptopId")]
        public virtual Laptop? Laptop { get; set; }

        /// <summary>
        /// Zimmeti alan kullanıcının ID'si
        /// </summary>
        [Required]
        [Display(Name = "User")]
        public int UserId { get; set; }

        /// <summary>
        /// Zimmeti alan kullanıcı nesnesi
        /// </summary>
        [ForeignKey("UserId")]
        public virtual User? User { get; set; }

        /// <summary>
        /// Zimmetin verildiği tarih
        /// </summary>
        [Required]
        [Display(Name = "Assignment Date")]
        public DateTime AssignmentDate { get; set; } = DateTime.Now;

        /// <summary>
        /// Zimmetin iade edildiği tarih (varsa)
        /// </summary>
        [Display(Name = "Return Date")]
        public DateTime? ReturnDate { get; set; }

        /// <summary>
        /// İşlem tarihi
        /// </summary>
        [Required]
        [Display(Name = "Date")]
        [DataType(DataType.DateTime)]
        public DateTime Tarih { get; set; } = DateTime.Now;

        /// <summary>
        /// İşlem tipi (Teslim, İade vb.)
        /// </summary>
        [Required]
        [StringLength(50)]
        [Display(Name = "Operation Type")]
        public string IslemTipi { get; set; } = string.Empty; // "Teslim", "İade", etc.
    }
}
