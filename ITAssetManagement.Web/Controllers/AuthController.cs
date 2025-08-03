using ITAssetManagement.Web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ITAssetManagement.Web.Controllers
{
    /// <summary>
    /// Kimlik doğrulama işlemlerini yöneten controller sınıfı
    /// </summary>
    public class AuthController : Controller
    {
        private readonly IConfiguration _configuration;

        /// <summary>
        /// AuthController constructor
        /// </summary>
        /// <param name="configuration">Uygulama yapılandırma servisi</param>
        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Giriş sayfasını gösterir
        /// </summary>
        /// <returns>Login view'i veya giriş yapmış kullanıcı için ana sayfa yönlendirmesi</returns>
        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        /// <summary>
        /// Kullanıcı girişi işlemini gerçekleştirir
        /// </summary>
        /// <param name="model">Kullanıcı giriş bilgileri</param>
        /// <returns>Başarılı ise ana sayfaya yönlendirir</returns>
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var username = Environment.GetEnvironmentVariable("USER_ACCOUNT");
                var password = Environment.GetEnvironmentVariable("USER_PASSWORD");

                if (model.Username == username && model.Password == password)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, model.Username),
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = true,
                        ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1)
                    };

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties);

                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", "Geçersiz kullanıcı adı veya şifre");
            }

            return View(model);
        }

        /// <summary>
        /// Kullanıcı çıkış işlemini gerçekleştirir
        /// </summary>
        /// <returns>Login sayfasına yönlendirme</returns>
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}
