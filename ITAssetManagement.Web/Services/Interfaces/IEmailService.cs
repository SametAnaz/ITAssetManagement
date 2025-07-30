using ITAssetManagement.Web.Models;
using ITAssetManagement.Web.Models.Email;

namespace ITAssetManagement.Web.Services.Interfaces
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(string to, string subject, string body, string? relatedEntityType = null, int? relatedEntityId = null);
        Task<bool> SendAssignmentReminderAsync(Assignment assignment);
        Task<bool> SendOverdueNotificationAsync(Assignment assignment);
        Task<EmailLog> LogEmailAsync(string to, string subject, string body, bool isSuccess, string? errorMessage = null, string? relatedEntityType = null, int? relatedEntityId = null);
    }
}