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
        /// <param name="pageNumber">Sayfa numarası</param>
        /// <param name="pageSize">Sayfa başına kayıt sayısı</param>
        /// <param name="sortBy">Sıralama yapılacak alan</param>
        /// <param name="sortDirection">Sıralama yönü (asc/desc)</param>
        /// <returns>Sayfalanmış laptop listesi ile ana sayfa view'i</returns>
        public async Task<IActionResult> Index(int? pageNumber, int? pageSize, string? sortBy, string? sortDirection)
        {
            // Sayfa başına kayıt sayısı seçenekleri
            var pageSizeOptions = new List<int> { 5, 10, 25, 50 };
            ViewBag.PageSizeOptions = pageSizeOptions;
            
            // Seçilen sayfa başına kayıt sayısı veya varsayılan değer (10)
            int currentPageSize = pageSize ?? 10;
            ViewBag.CurrentPageSize = currentPageSize;

            // Sayfa numarası veya varsayılan değer (1)
            int currentPageNumber = pageNumber ?? 1;

            // Sıralama parametreleri
            var currentSortBy = sortBy ?? "Id";
            var currentSortDirection = sortDirection ?? "asc";
            
            ViewBag.CurrentSortBy = currentSortBy;
            ViewBag.CurrentSortDirection = currentSortDirection;

            // Sıralama için ViewBag'e değerleri gönder
            ViewBag.IdSort = currentSortBy == "Id" ? (currentSortDirection == "asc" ? "desc" : "asc") : "asc";
            ViewBag.EtiketNoSort = currentSortBy == "EtiketNo" ? (currentSortDirection == "asc" ? "desc" : "asc") : "asc";
            ViewBag.MarkaSort = currentSortBy == "Marka" ? (currentSortDirection == "asc" ? "desc" : "asc") : "asc";
            ViewBag.ModelSort = currentSortBy == "Model" ? (currentSortDirection == "asc" ? "desc" : "asc") : "asc";
            ViewBag.DurumSort = currentSortBy == "Durum" ? (currentSortDirection == "asc" ? "desc" : "asc") : "asc";
            ViewBag.KayitTarihiSort = currentSortBy == "KayitTarihi" ? (currentSortDirection == "asc" ? "desc" : "asc") : "desc";

            // Entity Framework context üzerinden IQueryable alıyoruz
            var laptopsQuery = _laptopService.GetAllLaptopsQueryable();

            // Sıralama işlemi
            laptopsQuery = ApplySorting(laptopsQuery, currentSortBy, currentSortDirection);

            var laptops = await PaginatedList<Laptop>.CreateAsync(
                laptopsQuery.AsNoTracking(), // AsNoTracking ile performans optimizasyonu
                currentPageNumber,
                currentPageSize
            );

            return View(laptops);
        }

        /// <summary>
        /// Laptop listesine sıralama uygular
        /// </summary>
        /// <param name="query">Sıralanacak IQueryable</param>
        /// <param name="sortBy">Sıralama yapılacak alan</param>
        /// <param name="sortDirection">Sıralama yönü</param>
        /// <returns>Sıralanmış IQueryable</returns>
        private IQueryable<Laptop> ApplySorting(IQueryable<Laptop> query, string sortBy, string sortDirection)
        {
            var isDescending = sortDirection.ToLower() == "desc";

            return sortBy.ToLower() switch
            {
                "id" => isDescending ? query.OrderByDescending(l => l.Id) : query.OrderBy(l => l.Id),
                "etiketno" => isDescending ? query.OrderByDescending(l => l.EtiketNo) : query.OrderBy(l => l.EtiketNo),
                "marka" => isDescending ? query.OrderByDescending(l => l.Marka) : query.OrderBy(l => l.Marka),
                "model" => isDescending ? query.OrderByDescending(l => l.Model) : query.OrderBy(l => l.Model),
                "durum" => isDescending ? query.OrderByDescending(l => l.Durum) : query.OrderBy(l => l.Durum),
                "kayittarihi" => isDescending ? query.OrderByDescending(l => l.KayitTarihi) : query.OrderBy(l => l.KayitTarihi),
                _ => query.OrderBy(l => l.Id) // Varsayılan sıralama ID'ye göre artan
            };
        }
    }
}
