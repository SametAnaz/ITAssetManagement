using ITAssetManagement.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ITAssetManagement.Web.Controllers
{
    public class EmailTestController : Controller
    {
        private readonly IEmailService _emailService;
        private readonly ILogger<EmailTestController> _logger;

        public EmailTestController(
            IEmailService emailService,
            ILogger<EmailTestController> logger)
        {
            _emailService = emailService;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendTestEmail()
        {
            try
            {
                var result = await _emailService.SendEmailAsync(
                    "test@local.dev",
                    "Test Email from IT Asset Management",
                    @"<h1>Test Email</h1>
                    <p>Bu bir test emailidir.</p>
                    <p>IT Asset Management sisteminden gönderilmiştir.</p>
                    <br/>
                    <p>Tarih: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "</p>");

                if (result)
                {
                    TempData["SuccessMessage"] = "Test email başarıyla gönderildi!";
                    _logger.LogInformation("Test email sent successfully");
                }
                else
                {
                    TempData["ErrorMessage"] = "Test email gönderilemedi!";
                    _logger.LogError("Failed to send test email");
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Test email gönderilirken bir hata oluştu: " + ex.Message;
                _logger.LogError(ex, "Error sending test email");
            }

            return RedirectToAction(nameof(Index));
        }
    }
}