using ITAssetManagement.Web.Models;
using ITAssetManagement.Web.Models.Email;
using ITAssetManagement.Web.Services.Interfaces;
using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Net;
using ITAssetManagement.Web.Data;

namespace ITAssetManagement.Web.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailConfiguration _emailConfig;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<EmailService> _logger;

        public EmailService(
            IOptions<EmailConfiguration> emailConfig,
            ApplicationDbContext context,
            ILogger<EmailService> logger)
        {
            _emailConfig = emailConfig.Value;
            _context = context;
            _logger = logger;
        }

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