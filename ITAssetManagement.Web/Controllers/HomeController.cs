using ITAssetManagement.Web.Services.Interfaces;
using ITAssetManagement.Web.Models;
using Microsoft.AspNetCore.Mvc;
using ITAssetManagement.Web.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

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
        public async Task<IActionResult> Index(int? pageNumber, int? pageSize)
        {
            // Sayfa başına kayıt sayısı seçenekleri
            var pageSizeOptions = new List<int> { 5, 10, 25, 50 };
            ViewBag.PageSizeOptions = pageSizeOptions;
            
            // Seçilen sayfa başına kayıt sayısı veya varsayılan değer (10)
            int currentPageSize = pageSize ?? 10;
            ViewBag.CurrentPageSize = currentPageSize;

            // Sayfa numarası veya varsayılan değer (1)
            int currentPageNumber = pageNumber ?? 1;

            // Entity Framework context üzerinden IQueryable alıyoruz
            var laptopsQuery = _laptopService.GetAllLaptopsQueryable()
                .OrderByDescending(l => l.KayitTarihi); // Son eklenenler başta olsun

            var laptops = await PaginatedList<Laptop>.CreateAsync(
                laptopsQuery.AsNoTracking(), // AsNoTracking ile performans optimizasyonu
                currentPageNumber,
                currentPageSize
            );

            return View(laptops);
        }
    }
}
