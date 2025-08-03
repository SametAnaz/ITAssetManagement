using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITAssetManagement.Web.Models
{
    /// <summary>
    /// Laptop zimmet işlemlerini temsil eden sınıf.
    /// Bu sınıf, bir laptopun bir kullanıcıya zimmetlenmesi ve iade süreçlerini yönetir.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Bu sınıf aşağıdaki işlemleri destekler:
    /// <list type="bullet">
    /// <item><description>Yeni zimmet kaydı oluşturma</description></item>
    /// <item><description>Zimmet iade işlemi</description></item>
    /// <item><description>Zimmet geçmişi takibi</description></item>
    /// <item><description>Aktif zimmetlerin yönetimi</description></item>
    /// </list>
    /// </para>
    /// <para>
    /// İlişkili tablolar:
    /// <list type="bullet">
    /// <item><description>Laptops - Zimmetlenen cihaz bilgileri</description></item>
    /// <item><description>Users - Zimmeti alan kullanıcı bilgileri</description></item>
    /// </list>
    /// </para>
    /// </remarks>
    /// <example>
    /// Yeni bir zimmet kaydı oluşturma örneği:
    /// <code>
    /// var assignment = new Assignment
    /// {
    ///     LaptopId = 1,
    ///     UserId = 1,
    ///     AssignmentDate = DateTime.Now,
    ///     IslemTipi = "Teslim"
    /// };
    /// </code>
    /// </example>
    public class Assignment
    {
        /// <summary>
        /// Zimmet kaydının benzersiz kimlik numarası
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Zimmetlenen laptopun ID'si.
        /// Bu alan, zimmetlenen cihazı benzersiz şekilde tanımlar.
        /// </summary>
        /// <remarks>
        /// Foreign key olarak Laptop tablosuna referans verir.
        /// Her laptop aynı anda sadece bir kullanıcıya zimmetlenebilir.
        /// </remarks>
        [Required]
        public int LaptopId { get; set; }

        /// <summary>
        /// Zimmetlenen laptop nesnesi.
        /// Navigation property olarak laptop detaylarına erişim sağlar.
        /// </summary>
        /// <remarks>
        /// Lazy loading ile ihtiyaç durumunda yüklenir.
        /// Include() metodu ile eager loading de yapılabilir.
        /// </remarks>
        [ForeignKey("LaptopId")]
        public virtual Laptop? Laptop { get; set; }

        /// <summary>
        /// Zimmeti alan kullanıcının ID'si.
        /// Bu alan, zimmeti alan kişiyi benzersiz şekilde tanımlar.
        /// </summary>
        /// <remarks>
        /// Foreign key olarak User tablosuna referans verir.
        /// Bir kullanıcı birden fazla zimmete sahip olabilir.
        /// </remarks>
        [Required]
        [Display(Name = "User")]
        public int UserId { get; set; }

        /// <summary>
        /// Zimmeti alan kullanıcı nesnesi.
        /// Navigation property olarak kullanıcı detaylarına erişim sağlar.
        /// </summary>
        /// <remarks>
        /// Lazy loading ile ihtiyaç durumunda yüklenir.
        /// Include() metodu ile eager loading de yapılabilir.
        /// </remarks>
        [ForeignKey("UserId")]
        public virtual User? User { get; set; }

        /// <summary>
        /// Zimmetin verildiği tarih.
        /// Varsayılan olarak işlemin yapıldığı anı kaydeder.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Bu alan şu durumlarda önemlidir:
        /// <list type="bullet">
        /// <item><description>Zimmet süresinin takibi</description></item>
        /// <item><description>Raporlama ve istatistik</description></item>
        /// <item><description>Denetim ve kontrol</description></item>
        /// </list>
        /// </para>
        /// </remarks>
        [Required]
        [Display(Name = "Assignment Date")]
        public DateTime AssignmentDate { get; set; } = DateTime.Now;

        /// <summary>
        /// Zimmetin iade edildiği tarih (varsa).
        /// Null değer, zimmetin hala aktif olduğunu gösterir.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Bu alan şu amaçlarla kullanılır:
        /// <list type="bullet">
        /// <item><description>İade durumunun kontrolü</description></item>
        /// <item><description>Zimmet süresinin hesaplanması</description></item>
        /// <item><description>Aktif/Pasif zimmet filtresi</description></item>
        /// </list>
        /// </para>
        /// </remarks>
        [Display(Name = "Return Date")]
        public DateTime? ReturnDate { get; set; }

        /// <summary>
        /// İşlem tarihi.
        /// Her zimmet işleminin (veriliş/iade) gerçekleştiği anı kaydeder.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Bu alan şu amaçlarla kullanılır:
        /// <list type="bullet">
        /// <item><description>İşlem geçmişinin takibi</description></item>
        /// <item><description>Audit logging</description></item>
        /// <item><description>Kronolojik sıralama</description></item>
        /// </list>
        /// </para>
        /// </remarks>
        [Required]
        [Display(Name = "Date")]
        [DataType(DataType.DateTime)]
        public DateTime Tarih { get; set; } = DateTime.Now;

        /// <summary>
        /// İşlem tipi (Teslim, İade vb.).
        /// Zimmet işleminin türünü belirtir.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Geçerli işlem tipleri:
        /// <list type="bullet">
        /// <item><description>"Teslim" - Yeni zimmet verilişi</description></item>
        /// <item><description>"İade" - Zimmet iadesi</description></item>
        /// </list>
        /// </para>
        /// </remarks>
        /// <value>İşlem tipini belirten string değer</value>
        [Required]
        [StringLength(50)]
        [Display(Name = "Operation Type")]
        public string IslemTipi { get; set; } = string.Empty; // "Teslim", "İade", etc.
    }
}
