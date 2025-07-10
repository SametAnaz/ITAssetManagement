using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITAssetManagement.Web.Models
{
    public class DeletedLaptop
    {
        [Key]
        public int Id { get; set; }

        // Orijinal Laptop Bilgileri
        [Required]
        public int OriginalLaptopId { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Etiket No")]
        public string EtiketNo { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        [Display(Name = "Marka")]
        public string Marka { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        [Display(Name = "Model")]
        public string Model { get; set; } = string.Empty;

        [StringLength(1000)]
        [Display(Name = "Özellikler")]
        public string? Ozellikler { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Durum (Silinmeden Önceki)")]
        public string Durum { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Orijinal Kayıt Tarihi")]
        [DataType(DataType.DateTime)]
        public DateTime KayitTarihi { get; set; }

        // Silme İşlemi Bilgileri
        [Required]
        [Display(Name = "Silinme Tarihi")]
        [DataType(DataType.DateTime)]
        public DateTime SilinmeTarihi { get; set; } = DateTime.Now;

        [StringLength(100)]
        [Display(Name = "Silen Kullanıcı")]
        public string? SilenKullanici { get; set; }

        [StringLength(500)]
        [Display(Name = "Silme Nedeni")]
        public string? SilmeNedeni { get; set; }

        // TODO: Kullanıcı sistemi eklendikten sonra aktif edilecek
        // [ForeignKey("SilenKullanici")]
        // public virtual User? SilenKullaniciRef { get; set; }
    }
}
