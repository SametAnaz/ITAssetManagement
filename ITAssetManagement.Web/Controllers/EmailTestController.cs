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
                    "BT Varlık Yönetimi - Test E-postası",
                    @"<h1>Test E-postası</h1>
                    <p>Bu bir test e-postasıdır.</p>
                    <p>BT Varlık Yönetimi sisteminden gönderilmiştir.</p>
                    <br/>
                    <p>Tarih: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "</p>");

                if (result)
                {
                    TempData["SuccessMessage"] = "Test e-postası başarıyla gönderildi!";
                    _logger.LogInformation("Test e-postası başarıyla gönderildi");
                }
                else
                {
                    TempData["ErrorMessage"] = "Test e-postası gönderilemedi!";
                    _logger.LogError("Test e-postası gönderilemedi");
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Test e-postası gönderilirken bir hata oluştu: " + ex.Message;
                _logger.LogError(ex, "Test e-postası gönderilirken hata oluştu");
            }

            return RedirectToAction(nameof(Index));
        }
    }
}