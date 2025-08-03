using System.ComponentModel.DataAnnotations;

namespace ITAssetManagement.Web.Models
{
    /// <summary>
    /// Sistemdeki kullanıcıları temsil eden sınıf.
    /// Bu sınıf, IT varlıklarını zimmetinde bulunduran personel bilgilerini yönetir.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Bu sınıf aşağıdaki işlevleri sağlar:
    /// <list type="bullet">
    /// <item><description>Personel bilgilerinin yönetimi</description></item>
    /// <item><description>Zimmet takibi</description></item>
    /// <item><description>Departman bazlı raporlama</description></item>
    /// <item><description>İletişim bilgilerinin tutulması</description></item>
    /// </list>
    /// </para>
    /// <para>
    /// İlişkili tablolar:
    /// <list type="bullet">
    /// <item><description>Assignments - Kullanıcının zimmet kayıtları</description></item>
    /// </list>
    /// </para>
    /// </remarks>
    /// <example>
    /// Yeni bir kullanıcı kaydı oluşturma örneği:
    /// <code>
    /// var user = new User
    /// {
    ///     FullName = "John Doe",
    ///     Email = "john.doe@company.com",
    ///     Department = "IT",
    ///     Position = "Developer"
    /// };
    /// </code>
    /// </example>
    public class User
    {
        /// <summary>
        /// Kullanıcının benzersiz kimlik numarası.
        /// </summary>
        /// <remarks>
        /// Primary key olarak kullanılır ve otomatik artar.
        /// </remarks>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Kullanıcının tam adı.
        /// Personelin ad ve soyadını içerir.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Format kuralları:
        /// <list type="bullet">
        /// <item><description>Ad ve soyad boşluk ile ayrılır</description></item>
        /// <item><description>Maksimum 100 karakter</description></item>
        /// <item><description>Özel karakterler kullanılmamalı</description></item>
        /// </list>
        /// </para>
        /// </remarks>
        [Required]
        [StringLength(100)]
        [Display(Name = "Full Name")]
        public string FullName { get; set; } = string.Empty;

        /// <summary>
        /// Kullanıcının email adresi.
        /// İletişim ve bildirimler için kullanılır.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Email adresi şu amaçlarla kullanılır:
        /// <list type="bullet">
        /// <item><description>Zimmet bildirimleri</description></item>
        /// <item><description>Sistem bildirimleri</description></item>
        /// <item><description>İade hatırlatmaları</description></item>
        /// </list>
        /// </para>
        /// </remarks>
        [Required]
        [StringLength(100)]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Kullanıcının telefon numarası.
        /// Acil durumlarda iletişim için kullanılır.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Format önerileri:
        /// <list type="bullet">
        /// <item><description>Uluslararası format (+90...)</description></item>
        /// <item><description>Boşluk ve özel karaktersiz</description></item>
        /// <item><description>Maksimum 20 karakter</description></item>
        /// </list>
        /// </para>
        /// </remarks>
        [StringLength(20)]
        [Display(Name = "Phone")]
        public string? Phone { get; set; }

        /// <summary>
        /// Kullanıcının çalıştığı departman.
        /// Organizasyonel yapı içindeki konumunu belirtir.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Departman bilgisi şu amaçlarla kullanılır:
        /// <list type="bullet">
        /// <item><description>Zimmet raporlaması</description></item>
        /// <item><description>Departman bazlı envanter takibi</description></item>
        /// <item><description>Bütçe planlama</description></item>
        /// </list>
        /// </para>
        /// </remarks>
        [StringLength(100)]
        [Display(Name = "Department")]
        public string? Department { get; set; }

        /// <summary>
        /// Kullanıcının pozisyonu.
        /// Görev ve yetki seviyesini belirtir.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Pozisyon bilgisi şu amaçlarla kullanılır:
        /// <list type="bullet">
        /// <item><description>Zimmet yetkilendirmesi</description></item>
        /// <item><description>Cihaz tahsis politikaları</description></item>
        /// <item><description>Raporlama kategorileri</description></item>
        /// </list>
        /// </para>
        /// </remarks>
        [StringLength(100)]
        [Display(Name = "Position")]
        public string? Position { get; set; }

        /// <summary>
        /// Kullanıcıya zimmetlenmiş varlıkların listesi.
        /// Aktif ve geçmiş tüm zimmetleri içerir.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Bu koleksiyon şu bilgileri sağlar:
        /// <list type="bullet">
        /// <item><description>Mevcut zimmetler</description></item>
        /// <item><description>Zimmet geçmişi</description></item>
        /// <item><description>İade edilen cihazlar</description></item>
        /// </list>
        /// </para>
        /// </remarks>
        public List<Assignment> Assignments { get; set; } = new List<Assignment>();
    }
}
