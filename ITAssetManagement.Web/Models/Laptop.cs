using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITAssetManagement.Web.Models
{
    /// <summary>
    /// Sistemdeki laptop varlıklarını temsil eden sınıf.
    /// Bu sınıf, şirketin sahip olduğu tüm laptopların yönetimini ve takibini sağlar.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Bu sınıf aşağıdaki işlevleri sağlar:
    /// <list type="bullet">
    /// <item><description>Laptop envanteri yönetimi</description></item>
    /// <item><description>Teknik özelliklerin takibi</description></item>
    /// <item><description>Durumların ve zimmet bilgilerinin yönetimi</description></item>
    /// <item><description>Fotoğraf ve log kayıtlarının tutulması</description></item>
    /// </list>
    /// </para>
    /// <para>
    /// İlişkili tablolar:
    /// <list type="bullet">
    /// <item><description>LaptopPhotos - Laptop fotoğrafları</description></item>
    /// <item><description>LaptopLogs - İşlem kayıtları</description></item>
    /// <item><description>Assignments - Zimmet kayıtları</description></item>
    /// </list>
    /// </para>
    /// </remarks>
    /// <example>
    /// Yeni bir laptop kaydı oluşturma örneği:
    /// <code>
    /// var laptop = new Laptop
    /// {
    ///     EtiketNo = "LP001",
    ///     Marka = "Dell",
    ///     Model = "Latitude 5520",
    ///     Ozellikler = "i7, 16GB RAM, 512GB SSD",
    ///     Durum = "Aktif"
    /// };
    /// </code>
    /// </example>
    public class Laptop
    {
        /// <summary>
        /// Laptopun benzersiz kimlik numarası
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Laptopun etiket numarası.
        /// Bu numara, envanter takibi için kullanılan benzersiz bir tanımlayıcıdır.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Etiket numarası şu amaçlarla kullanılır:
        /// <list type="bullet">
        /// <item><description>Fiziksel envanter sayımı</description></item>
        /// <item><description>Barkod/QR kod etiketleme</description></item>
        /// <item><description>Hızlı laptop tanımlama</description></item>
        /// </list>
        /// </para>
        /// </remarks>
        [Required]
        [StringLength(50)]
        [Display(Name = "Tag Number")]
        public string EtiketNo { get; set; } = string.Empty;

        /// <summary>
        /// Laptopun görüntülenen adı (Marka Model (EtiketNo)).
        /// UI'da kolay tanımlama için kullanılan birleşik ad.
        /// </summary>
        /// <remarks>
        /// NotMapped attribute'u ile veritabanında saklanmaz,
        /// runtime'da diğer alanlardan oluşturulur.
        /// </remarks>
        /// <example>"Dell Latitude 5520 (LP001)"</example>
        [NotMapped]
        public string DisplayName => $"{Marka} {Model} ({EtiketNo})";

        /// <summary>
        /// Laptopun markası
        /// </summary>
        [Required]
        [StringLength(100)]
        [Display(Name = "Brand")]
        public string Marka { get; set; } = string.Empty;

        /// <summary>
        /// Laptopun modeli
        /// </summary>
        [Required]
        [StringLength(100)]
        [Display(Name = "Model")]
        public string Model { get; set; } = string.Empty;

        /// <summary>
        /// Laptopun teknik özellikleri
        /// </summary>
        [StringLength(1000)]
        [Display(Name = "Specifications")]
        public string? Ozellikler { get; set; }

        /// <summary>
        /// Laptopun durumu (Aktif, Bakımda, Hurda vb.).
        /// Cihazın mevcut kullanım durumunu belirtir.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Geçerli durum değerleri:
        /// <list type="bullet">
        /// <item><description>"Aktif" - Kullanımda veya kullanıma hazır</description></item>
        /// <item><description>"Bakımda" - Onarım veya bakım sürecinde</description></item>
        /// <item><description>"Hurda" - Kullanım dışı, imha edilecek</description></item>
        /// <item><description>"Kayıp" - Lokasyonu bilinmiyor</description></item>
        /// </list>
        /// </para>
        /// </remarks>
        [Required]
        [StringLength(50)]
        [Display(Name = "Status")]
        public string Durum { get; set; } = string.Empty;

        /// <summary>
        /// Laptopun sisteme kayıt tarihi
        /// </summary>
        [Required]
        [Display(Name = "Registration Date")]
        [DataType(DataType.DateTime)]
        public DateTime KayitTarihi { get; set; } = DateTime.Now;

        /// <summary>
        /// Laptopun aktif olup olmadığını belirten flag
        /// </summary>
        [Required]
        [Display(Name = "Is Active")]
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Laptopun silinme nedeni.
        /// Envanter dışına çıkarılan cihazlar için açıklama.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Bu alan şu durumlarda doldurulur:
        /// <list type="bullet">
        /// <item><description>Hurda durumuna geçiş</description></item>
        /// <item><description>Çalınma/Kayıp</description></item>
        /// <item><description>Başka birime devir</description></item>
        /// </list>
        /// </para>
        /// </remarks>
        [Display(Name = "Deletion Reason")]
        [StringLength(500)]
        public string? SilmeNedeni { get; set; }

        [Display(Name = "Deletion Date")]
        [DataType(DataType.DateTime)]
        public DateTime? SilinmeTarihi { get; set; }

        [Display(Name = "Deleted By")]
        [StringLength(100)]
        public string? SilenKullanici { get; set; }

        [Display(Name = "Notes")]
        [StringLength(1000)]
        [DataType(DataType.MultilineText)]
        public string? Notes { get; set; }

        /// <summary>
        /// Laptop ile ilişkili fotoğraflar koleksiyonu.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Fotoğraflar şu amaçlarla kullanılır:
        /// <list type="bullet">
        /// <item><description>Cihaz durumunun belgelenmesi</description></item>
        /// <item><description>Hasar tespiti</description></item>
        /// <item><description>Zimmet teslim/iade süreçleri</description></item>
        /// </list>
        /// </para>
        /// </remarks>
        [Display(Name = "Photos")]
        public virtual ICollection<LaptopPhoto>? Photos { get; set; }

        /// <summary>
        /// Laptop ile ilgili log kayıtları koleksiyonu.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Loglar şu işlemleri kaydeder:
        /// <list type="bullet">
        /// <item><description>Zimmet işlemleri</description></item>
        /// <item><description>Durum değişiklikleri</description></item>
        /// <item><description>Bakım/Onarım kayıtları</description></item>
        /// <item><description>Envanter güncellemeleri</description></item>
        /// </list>
        /// </para>
        /// </remarks>
        [Display(Name = "Logs")]
        public virtual ICollection<LaptopLog>? Loglar { get; set; }

        /// <summary>
        /// Laptopun mevcut zimmet kaydı.
        /// Eğer varsa, laptopun kime zimmetli olduğunu gösterir.
        /// </summary>
        /// <remarks>
        /// Null değer, laptopun herhangi bir kullanıcıya zimmetli olmadığını gösterir.
        /// </remarks>
        [Display(Name = "Current Assignment")]
        public virtual Assignment? CurrentAssignment { get; set; }

        // Constructor to initialize collections
        public Laptop()
        {
            Photos = new List<LaptopPhoto>();
            Loglar = new List<LaptopLog>();
        }
    }
}
