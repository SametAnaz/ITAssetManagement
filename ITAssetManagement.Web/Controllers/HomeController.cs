using ITAssetManagement.Web.Services.Interfaces;
using ITAssetManagement.Web.Models;
using Microsoft.AspNetCore.Mvc;
using ITAssetManagement.Web.Extensions;
using Microsoft.AspNetCore.Authorization;

namespace ITAssetManagement.Web.Controllers
{
    /// <summary>
    /// Ana sayfa işlemlerini yöneten controller sınıfı
    /// </summary>
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILaptopService _laptopService;

        /// <summary>
        /// HomeController constructor
        /// </summary>
        /// <param name="laptopService">Laptop işlemleri servisi</param>
        public HomeController(ILaptopService laptopService)
        {
            _laptopService = laptopService;
        }

        /// <summary>
        /// Ana sayfayı ve sayfalanmış laptop listesini gösterir
        /// </summary>
        /// <param name="page">Sayfa numarası</param>
        /// <returns>Sayfalanmış laptop listesi ile ana sayfa view'i</returns>
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
