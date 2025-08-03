using ITAssetManagement.Web.Models;
using ITAssetManagement.Web.Models.Email;

namespace ITAssetManagement.Web.Services.Interfaces
{
    /// <summary>
    /// Email gönderme ve loglama işlemlerini yöneten servis arayüzü.
    /// Bu servis, sistem genelinde email iletişimini ve kayıtlarını yönetir.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Bu servis aşağıdaki temel işlevleri sağlar:
    /// <list type="bullet">
    /// <item><description>Genel email gönderimi</description></item>
    /// <item><description>Zimmet hatırlatmaları</description></item>
    /// <item><description>Gecikme bildirimleri</description></item>
    /// <item><description>Email loglama ve takip</description></item>
    /// </list>
    /// </para>
    /// </remarks>
    public interface IEmailService
    {
        /// <summary>
        /// Genel amaçlı email gönderme metodu.
        /// </summary>
        /// <param name="to">Alıcı email adresi</param>
        /// <param name="subject">Email konusu</param>
        /// <param name="body">Email içeriği</param>
        /// <param name="relatedEntityType">İlişkili varlık tipi (opsiyonel)</param>
        /// <param name="relatedEntityId">İlişkili varlık ID'si (opsiyonel)</param>
        /// <returns>Email gönderimi başarılı ise true</returns>
        /// <remarks>
        /// <para>
        /// Bu metot:
        /// <list type="bullet">
        /// <item><description>HTML formatında email gönderir</description></item>
        /// <item><description>Gönderim sonucunu loglar</description></item>
        /// <item><description>Hata durumunda retry mekanizması çalıştırır</description></item>
        /// </list>
        /// </para>
        /// </remarks>
        Task<bool> SendEmailAsync(string to, string subject, string body, string? relatedEntityType = null, int? relatedEntityId = null);

        /// <summary>
        /// Zimmet süresi yaklaşan kullanıcılara hatırlatma emaili gönderir.
        /// </summary>
        /// <param name="assignment">İlgili zimmet kaydı</param>
        /// <returns>Email gönderimi başarılı ise true</returns>
        /// <remarks>
        /// <para>
        /// Bu metot şu durumlarda kullanılır:
        /// <list type="bullet">
        /// <item><description>Zimmet bitiş tarihi yaklaşanlar</description></item>
        /// <item><description>İade süresi gecikmiş olanlar</description></item>
        /// <item><description>Periyodik hatırlatmalar</description></item>
        /// </list>
        /// </para>
        /// </remarks>
        Task<bool> SendAssignmentReminderAsync(Assignment assignment);

        /// <summary>
        /// Zimmet süresi geçmiş kullanıcılara bildirim emaili gönderir.
        /// </summary>
        /// <param name="assignment">İlgili zimmet kaydı</param>
        /// <returns>Email gönderimi başarılı ise true</returns>
        /// <remarks>
        /// <para>
        /// Bu metot şu özelliklere sahiptir:
        /// <list type="bullet">
        /// <item><description>Yöneticileri CC'ye ekler</description></item>
        /// <item><description>Gecikme süresini hesaplar</description></item>
        /// <item><description>Aciliyet durumuna göre önceliklendirme yapar</description></item>
        /// </list>
        /// </para>
        /// </remarks>
        Task<bool> SendOverdueNotificationAsync(Assignment assignment);

        /// <summary>
        /// Email gönderim işlemlerini loglar.
        /// </summary>
        /// <param name="to">Alıcı email adresi</param>
        /// <param name="subject">Email konusu</param>
        /// <param name="body">Email içeriği</param>
        /// <param name="isSuccess">Gönderim başarılı mı?</param>
        /// <param name="errorMessage">Hata mesajı (başarısız durumda)</param>
        /// <param name="relatedEntityType">İlişkili varlık tipi (opsiyonel)</param>
        /// <param name="relatedEntityId">İlişkili varlık ID'si (opsiyonel)</param>
        /// <returns>Oluşturulan log kaydı</returns>
        /// <remarks>
        /// <para>
        /// Log kaydı şu bilgileri içerir:
        /// <list type="bullet">
        /// <item><description>Gönderim zamanı ve durumu</description></item>
        /// <item><description>Başarısızlık nedeni ve retry sayısı</description></item>
        /// <item><description>İlişkili varlık referansları</description></item>
        /// </list>
        /// </para>
        /// </remarks>
        Task<EmailLog> LogEmailAsync(string to, string subject, string body, bool isSuccess, string? errorMessage = null, string? relatedEntityType = null, int? relatedEntityId = null);
    }
}