using ITAssetManagement.Web.Services.Interfaces;
using ITAssetManagement.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace ITAssetManagement.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILaptopService _laptopService;

        public HomeController(ILaptopService laptopService)
        {
            _laptopService = laptopService;
        }

        public async Task<IActionResult> Index()
        {
            var laptops = await _laptopService.GetAllLaptopsAsync();
            return View(laptops);
        }
    }
}
