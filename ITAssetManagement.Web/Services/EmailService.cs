using ITAssetManagement.Web.Models;
using ITAssetManagement.Web.Models.Email;
using ITAssetManagement.Web.Services.Interfaces;
using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Net;
using ITAssetManagement.Web.Data;

namespace ITAssetManagement.Web.Services
{
    /// <summary>
    /// Email gönderme ve loglama işlemlerini yöneten servis sınıfı.
    /// IEmailService arayüzünü implement eder.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Bu servis şu işlevleri sağlar:
    /// <list type="bullet">
    /// <item><description>SMTP üzerinden email gönderimi</description></item>
    /// <item><description>Zimmet hatırlatma bildirimleri</description></item>
    /// <item><description>Gecikme bildirimleri</description></item>
    /// <item><description>Email gönderim logları</description></item>
    /// </list>
    /// </para>
    /// </remarks>
    public class EmailService : IEmailService
    {
        private readonly EmailConfiguration _emailConfig;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<EmailService> _logger;

        /// <summary>
        /// EmailService sınıfının yeni bir instance'ını oluşturur.
        /// </summary>
        /// <param name="emailConfig">Email sunucu yapılandırması</param>
        /// <param name="context">Veritabanı context'i</param>
        /// <param name="logger">Loglama servisi</param>
        /// <remarks>
        /// Dependency injection ile gerekli bağımlılıkları alır.
        /// </remarks>
        public EmailService(
            IOptions<EmailConfiguration> emailConfig,
            ApplicationDbContext context,
            ILogger<EmailService> logger)
        {
            _emailConfig = emailConfig.Value;
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Email gönderme işlemini gerçekleştirir.
        /// </summary>
        /// <param name="to">Alıcı email adresi</param>
        /// <param name="subject">Email konusu</param>
        /// <param name="body">Email içeriği (HTML formatında)</param>
        /// <param name="relatedEntityType">İlişkili varlık tipi (opsiyonel)</param>
        /// <param name="relatedEntityId">İlişkili varlık ID'si (opsiyonel)</param>
        /// <returns>Gönderim başarılı ise true</returns>
        /// <remarks>
        /// <para>
        /// Bu metot şu işlemleri gerçekleştirir:
        /// <list type="bullet">
        /// <item><description>SMTP bağlantısı kurulması</description></item>
        /// <item><description>Email formatının hazırlanması</description></item>
        /// <item><description>Gönderim işlemi</description></item>
        /// <item><description>Hata yönetimi ve loglama</description></item>
        /// </list>
        /// </para>
        /// </remarks>
        public async Task<bool> SendEmailAsync(string to, string subject, string body, string? relatedEntityType = null, int? relatedEntityId = null)
        {
            try
            {
                using var client = new SmtpClient(_emailConfig.SmtpHost, _emailConfig.SmtpPort)
                {
                    Credentials = new NetworkCredential(_emailConfig.SmtpUsername, _emailConfig.SmtpPassword),
                    EnableSsl = _emailConfig.EnableSsl
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_emailConfig.FromEmail, _emailConfig.FromName),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };

                mailMessage.To.Add(to);

                await client.SendMailAsync(mailMessage);
                await LogEmailAsync(to, subject, body, true, null, relatedEntityType, relatedEntityId);
                
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Email sending failed to {To} with subject {Subject}", to, subject);
                await LogEmailAsync(to, subject, body, false, ex.Message, relatedEntityType, relatedEntityId);
                
                return false;
            }
        }

        /// <summary>
        /// Zimmet süresi yaklaşan kullanıcılara hatırlatma emaili gönderir.
        /// </summary>
        /// <param name="assignment">İlgili zimmet kaydı</param>
        /// <returns>Gönderim başarılı ise true</returns>
        /// <remarks>
        /// <para>
        /// Email içeriği şu bilgileri içerir:
        /// <list type="bullet">
        /// <item><description>Kalan gün sayısı</description></item>
        /// <item><description>Laptop bilgileri</description></item>
        /// <item><description>İade tarihi</description></item>
        /// <item><description>İletişim bilgileri</description></item>
        /// </list>
        /// </para>
        /// </remarks>
        public async Task<bool> SendAssignmentReminderAsync(Assignment assignment)
        {
            if (assignment.User == null || string.IsNullOrEmpty(assignment.User.Email))
                return false;

            var daysLeft = (assignment.ReturnDate?.Date - DateTime.Now.Date)?.Days ?? 0;
            
            var subject = $"Reminder: Laptop Return Due in {daysLeft} Days";
            var body = $@"
                <h2>Laptop Return Reminder</h2>
                <p>Dear {assignment.User.FullName},</p>
                <p>This is a reminder that the laptop assigned to you is due for return in {daysLeft} days.</p>
                <h3>Details:</h3>
                <ul>
                    <li>Laptop: {assignment.Laptop?.Marka} {assignment.Laptop?.Model}</li>
                    <li>Tag Number: {assignment.Laptop?.EtiketNo}</li>
                    <li>Return Date: {assignment.ReturnDate:dd/MM/yyyy}</li>
                </ul>
                <p>Please ensure to return the laptop to the IT department by the due date.</p>
                <p>If you need an extension, please contact the IT department.</p>
                <br/>
                <p>Best regards,<br/>IT Asset Management Team</p>";

            return await SendEmailAsync(
                assignment.User.Email,
                subject,
                body,
                "Assignment",
                assignment.Id);
        }

        /// <summary>
        /// Zimmet süresi geçmiş kullanıcılara bildirim emaili gönderir.
        /// </summary>
        /// <param name="assignment">İlgili zimmet kaydı</param>
        /// <returns>Gönderim başarılı ise true</returns>
        /// <remarks>
        /// <para>
        /// Email içeriği şu bilgileri içerir:
        /// <list type="bullet">
        /// <item><description>Gecikme süresi</description></item>
        /// <item><description>Laptop bilgileri</description></item>
        /// <item><description>Son tarih bilgisi</description></item>
        /// <item><description>Acil iade talebi</description></item>
        /// </list>
        /// </para>
        /// </remarks>
        public async Task<bool> SendOverdueNotificationAsync(Assignment assignment)
        {
            if (assignment.User == null || string.IsNullOrEmpty(assignment.User.Email))
                return false;

            var daysOverdue = (DateTime.Now.Date - assignment.ReturnDate?.Date)?.Days ?? 0;
            
            var subject = $"OVERDUE: Laptop Return {daysOverdue} Days Late";
            var body = $@"
                <h2>Overdue Laptop Return Notice</h2>
                <p>Dear {assignment.User.FullName},</p>
                <p>The laptop assigned to you is <strong>{daysOverdue} days overdue</strong> for return.</p>
                <h3>Details:</h3>
                <ul>
                    <li>Laptop: {assignment.Laptop?.Marka} {assignment.Laptop?.Model}</li>
                    <li>Tag Number: {assignment.Laptop?.EtiketNo}</li>
                    <li>Due Date: {assignment.ReturnDate:dd/MM/yyyy}</li>
                    <li>Days Overdue: {daysOverdue}</li>
                </ul>
                <p>Please return the laptop to the IT department immediately.</p>
                <p>If you have already returned the laptop or need to discuss this matter, please contact the IT department.</p>
                <br/>
                <p>Best regards,<br/>IT Asset Management Team</p>";

            return await SendEmailAsync(
                assignment.User.Email,
                subject,
                body,
                "Assignment",
                assignment.Id);
        }

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
        /// Log kaydında şu bilgiler tutulur:
        /// <list type="bullet">
        /// <item><description>Gönderim zamanı ve durumu</description></item>
        /// <item><description>Alıcı ve içerik bilgileri</description></item>
        /// <item><description>Hata detayları (varsa)</description></item>
        /// <item><description>İlişkili varlık referansları</description></item>
        /// </list>
        /// </para>
        /// </remarks>
        public async Task<EmailLog> LogEmailAsync(string to, string subject, string body, bool isSuccess, string? errorMessage = null, string? relatedEntityType = null, int? relatedEntityId = null)
        {
            var log = new EmailLog
            {
                ToEmail = to,
                Subject = subject,
                Body = body,
                IsSuccess = isSuccess,
                ErrorMessage = errorMessage,
                RelatedEntityType = relatedEntityType,
                RelatedEntityId = relatedEntityId,
                SentDate = DateTime.Now
            };

            _context.Add(log);
            await _context.SaveChangesAsync();

            return log;
        }
    }
}