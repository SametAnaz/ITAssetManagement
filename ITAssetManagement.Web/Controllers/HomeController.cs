using ITAssetManagement.Web.Services.Interfaces;
using ITAssetManagement.Web.Models;
using Microsoft.AspNetCore.Mvc;
using ITAssetManagement.Web.Extensions;

namespace ITAssetManagement.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILaptopService _laptopService;

        public HomeController(ILaptopService laptopService)
        {
            _laptopService = laptopService;
        }

        public async Task<IActionResult> Index(int? page)
        {
            int pageSize = 10;
            int pageNumber = page ?? 1;

            var laptopsQuery = _laptopService.GetAllLaptopsQueryable();
            var laptops = await PaginatedList<Laptop>.CreateAsync(laptopsQuery, pageNumber, pageSize);

            ViewData["CurrentPage"] = pageNumber;
            return View(laptops);
        }
    }
}
