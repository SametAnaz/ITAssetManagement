using ITAssetManagement.Web.Services.Interfaces;
using ITAssetManagement.Web.Services;
using ITAssetManagement.Web.Models;
using Microsoft.AspNetCore.Mvc;
using ITAssetManagement.Web.Extensions;
using Microsoft.AspNetCore.Authorization;

namespace ITAssetManagement.Web.Controllers
{
    /// <summary>
    /// Laptop işlemlerini yöneten controller sınıfı
    /// </summary>
    [Authorize]
    public class LaptopsController : Controller
    {
        private readonly ILaptopService _laptopService;
        private readonly IBarcodeService _barcodeService;
        private readonly IBrandService _brandService;

        /// <summary>
        /// LaptopsController constructor
        /// </summary>
        /// <param name="laptopService">Laptop işlemleri servisi</param>
        /// <param name="barcodeService">Barkod işlemleri servisi</param>
        /// <param name="brandService">Marka işlemleri servisi</param>
        public LaptopsController(ILaptopService laptopService, IBarcodeService barcodeService, IBrandService brandService)
        {
            _laptopService = laptopService;
            _barcodeService = barcodeService;
            _brandService = brandService;
        }

        /// <summary>
        /// Laptop listesini sayfalı şekilde gösterir ve arama yapmayı sağlar
        /// </summary>
        /// <param name="searchTerm">Arama terimi</param>
        /// <param name="pageNumber">Sayfa numarası</param>
        /// <param name="pageSize">Sayfa başına kayıt sayısı</param>
        /// <param name="sortBy">Sıralama yapılacak alan</param>
        /// <param name="sortDirection">Sıralama yönü (asc/desc)</param>
        /// <returns>Sayfalanmış laptop listesi view'i</returns>
        public async Task<IActionResult> Index(string searchTerm, int? pageNumber, int? pageSize, string? sortBy, string? sortDirection)
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

            // Arama terimini ViewBag'e ekle
            ViewData["CurrentFilter"] = searchTerm;
            ViewData["CurrentSearch"] = searchTerm; // URL'lerde kullanmak için

            // Sorguyu oluştur
            var laptopsQuery = string.IsNullOrEmpty(searchTerm)
                ? _laptopService.GetAllLaptopsQueryable()
                : _laptopService.SearchLaptopsQueryable(searchTerm);

            // Sıralama işlemi
            laptopsQuery = ApplySorting(laptopsQuery, currentSortBy, currentSortDirection);

            // Sadece aktif laptopları göster ve sayfalama yap
            return View(await PaginatedList<Laptop>.CreateAsync(
                laptopsQuery.Where(l => l.IsActive), 
                currentPageNumber,
                currentPageSize));
        }

        /// <summary>
        /// Belirli bir laptop'un detaylarını gösterir
        /// </summary>
        /// <param name="id">Laptop ID</param>
        /// <returns>Laptop detay view'i</returns>
        public async Task<IActionResult> Details(int id)
        {
            var laptop = await _laptopService.GetLaptopWithDetailsAsync(id);
            if (laptop == null)
            {
                return NotFound();
            }
            return View(laptop);
        }

        /// <summary>
        /// Yeni laptop ekleme formunu gösterir
        /// </summary>
        /// <returns>Laptop ekleme view'i</returns>
        public async Task<IActionResult> Create()
        {
            var brands = await _brandService.GetAllActiveBrandsAsync();
            ViewBag.Brands = brands;
            return View();
        }

        /// <summary>
        /// Yeni laptop ekler
        /// </summary>
        /// <param name="laptop">Eklenecek laptop bilgileri</param>
        /// <returns>Başarılı ise Index sayfasına yönlendirir</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Laptop laptop)
        {
            if (ModelState.IsValid)
            {
                // EtiketNo boş ise hata döndür
                if (string.IsNullOrWhiteSpace(laptop.EtiketNo))
                {
                    ModelState.AddModelError("EtiketNo", "Etiket numarası gereklidir.");
                    var brands = await _brandService.GetAllActiveBrandsAsync();
                    ViewBag.Brands = brands;
                    return View(laptop);
                }

                // EtiketNo'nun benzersiz olup olmadığını kontrol et
                var existingLaptop = await _laptopService.GetAllLaptopsAsync();
                if (existingLaptop.Any(l => l.EtiketNo.Equals(laptop.EtiketNo, StringComparison.OrdinalIgnoreCase)))
                {
                    ModelState.AddModelError("EtiketNo", "Bu etiket numarası zaten kullanımda.");
                    var brands = await _brandService.GetAllActiveBrandsAsync();
                    ViewBag.Brands = brands;
                    return View(laptop);
                }
                
                // BrandId seçildiyse, Brand adını string Marka alanına da kopyala (backward compatibility için)
                if (laptop.BrandId.HasValue && laptop.BrandId.Value > 0)
                {
                    var selectedBrand = await _brandService.GetBrandByIdAsync(laptop.BrandId.Value);
                    if (selectedBrand != null)
                    {
                        laptop.Marka = selectedBrand.Name;
                    }
                }
                else
                {
                    // BrandId yoksa veya 0 ise, boş string koy
                    laptop.Marka = string.Empty;
                }
                
                await _laptopService.CreateLaptopAsync(laptop);
                TempData["SuccessMessage"] = "Laptop başarıyla eklendi.";
                return RedirectToAction(nameof(Index));
            }
            var brandsForError = await _brandService.GetAllActiveBrandsAsync();
            ViewBag.Brands = brandsForError;
            return View(laptop);
        }

        // GET: Laptops/DownloadBarcode/5
        public async Task<IActionResult> DownloadBarcode(int id)
        {
            var laptop = await _laptopService.GetLaptopByIdAsync(id);
            if (laptop == null || string.IsNullOrEmpty(laptop.EtiketNo))
            {
                return NotFound("Laptop veya EtiketNo bulunamadı.");
            }

            var barcodeStream = _barcodeService.GenerateBarcode(laptop.EtiketNo);
            return File(barcodeStream, "image/png", $"barcode-{laptop.EtiketNo}.png");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var laptop = await _laptopService.GetLaptopByIdAsync(id);
            if (laptop == null)
            {
                return NotFound();
            }
            var brands = await _brandService.GetAllActiveBrandsAsync();
            ViewBag.Brands = brands;
            return View(laptop);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Laptop laptop)
        {
            if (id != laptop.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // BrandId seçildiyse, Brand adını string Marka alanına da kopyala (backward compatibility için)
                if (laptop.BrandId.HasValue)
                {
                    var selectedBrand = await _brandService.GetBrandByIdAsync(laptop.BrandId.Value);
                    if (selectedBrand != null)
                    {
                        laptop.Marka = selectedBrand.Name;
                    }
                }
                
                await _laptopService.UpdateLaptopAsync(laptop);
                TempData["SuccessMessage"] = "Laptop başarıyla güncellendi.";
                return RedirectToAction(nameof(Index));
            }
            var brands = await _brandService.GetAllActiveBrandsAsync();
            ViewBag.Brands = brands;
            return View(laptop);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var laptop = await _laptopService.GetLaptopByIdAsync(id);
            if (laptop == null)
            {
                return NotFound();
            }
            return View(laptop);
        }

        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, string silmeNedeni)
        {
            if (string.IsNullOrWhiteSpace(silmeNedeni))
            {
                TempData["ErrorMessage"] = "Silme nedeni belirtilmesi zorunludur.";
                return RedirectToAction("Delete", new { id = id });
            }

            var result = await _laptopService.DeleteLaptopAsync(id, silmeNedeni);
            if (result)
            {
                TempData["SuccessMessage"] = "Laptop başarıyla silindi.";
            }
            else
            {
                TempData["ErrorMessage"] = "Laptop silinirken bir hata oluştu.";
            }
            
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> DeletedLaptops(string searchTerm, int? pageNumber, int? pageSize, string? sortBy, string? sortDirection)
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
            var currentSortBy = sortBy ?? "SilinmeTarihi";
            var currentSortDirection = sortDirection ?? "desc";
            
            ViewBag.CurrentSortBy = currentSortBy;
            ViewBag.CurrentSortDirection = currentSortDirection;

            // Sıralama için ViewBag'e değerleri gönder
            ViewBag.IdSort = currentSortBy == "Id" ? (currentSortDirection == "asc" ? "desc" : "asc") : "asc";
            ViewBag.EtiketNoSort = currentSortBy == "EtiketNo" ? (currentSortDirection == "asc" ? "desc" : "asc") : "asc";
            ViewBag.MarkaSort = currentSortBy == "Marka" ? (currentSortDirection == "asc" ? "desc" : "asc") : "asc";
            ViewBag.ModelSort = currentSortBy == "Model" ? (currentSortDirection == "asc" ? "desc" : "asc") : "asc";
            ViewBag.DurumSort = currentSortBy == "Durum" ? (currentSortDirection == "asc" ? "desc" : "asc") : "asc";
            ViewBag.KayitTarihiSort = currentSortBy == "KayitTarihi" ? (currentSortDirection == "asc" ? "desc" : "asc") : "desc";
            ViewBag.SilinmeTarihiSort = currentSortBy == "SilinmeTarihi" ? (currentSortDirection == "asc" ? "desc" : "asc") : "desc";
            ViewBag.SilmeNedeniSort = currentSortBy == "SilmeNedeni" ? (currentSortDirection == "asc" ? "desc" : "asc") : "asc";
            ViewBag.SilenKullaniciSort = currentSortBy == "SilenKullanici" ? (currentSortDirection == "asc" ? "desc" : "asc") : "asc";

            // Arama terimini ViewBag'e ekle
            ViewData["CurrentFilter"] = searchTerm;
            ViewData["CurrentSearch"] = searchTerm; // URL'lerde kullanmak için

            // Sorguyu oluştur
            var deletedLaptopsQuery = string.IsNullOrEmpty(searchTerm) 
                ? _laptopService.GetDeletedLaptopsQueryable()
                : _laptopService.SearchDeletedLaptopsQueryable(searchTerm);

            // Sıralama işlemi
            deletedLaptopsQuery = ApplyDeletedLaptopsSorting(deletedLaptopsQuery, currentSortBy, currentSortDirection);

            var paginatedLaptops = await PaginatedList<Laptop>.CreateAsync(
                deletedLaptopsQuery,
                currentPageNumber,
                currentPageSize
            );

            return View(paginatedLaptops);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RestoreLaptop(int id)
        {
            var result = await _laptopService.RestoreLaptopAsync(id);
            if (result)
            {
                TempData["SuccessMessage"] = "Laptop başarıyla geri yüklendi.";
            }
            else
            {
                TempData["ErrorMessage"] = "Laptop geri yüklenirken bir hata oluştu.";
            }
            
            return RedirectToAction(nameof(DeletedLaptops));
        }

        /// <summary>
        /// Laptop verilerini Excel formatında export eder
        /// </summary>
        /// <returns>Excel dosyası</returns>
        public async Task<IActionResult> ExportToExcel()
        {
            try
            {
                var excelData = await _laptopService.ExportLaptopsToExcelAsync();
                var fileName = $"Laptops_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
                return File(excelData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Export sırasında hata oluştu: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        /// <summary>
        /// Laptop verilerini CSV formatında export eder
        /// </summary>
        /// <returns>CSV dosyası</returns>
        public async Task<IActionResult> ExportToCsv()
        {
            try
            {
                var csvData = await _laptopService.ExportLaptopsToCsvAsync();
                var fileName = $"Laptops_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
                return File(csvData, "text/csv", fileName);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Export sırasında hata oluştu: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        /// <summary>
        /// Laptop verilerini dosyadan import eder
        /// </summary>
        /// <param name="file">Upload edilen dosya</param>
        /// <returns>Import sonucu</returns>
        [HttpPost]
        public async Task<IActionResult> Import(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                TempData["ErrorMessage"] = "Lütfen geçerli bir dosya seçin.";
                return RedirectToAction(nameof(Index));
            }

            if (!file.FileName.EndsWith(".csv") && !file.FileName.EndsWith(".xlsx"))
            {
                TempData["ErrorMessage"] = "Sadece CSV ve Excel dosyaları desteklenir.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                using var memoryStream = new MemoryStream();
                await file.CopyToAsync(memoryStream);
                var fileBytes = memoryStream.ToArray();

                var result = await _laptopService.ImportLaptopsFromFileAsync(fileBytes, file.FileName);
                
                if (result.Success)
                {
                    TempData["SuccessMessage"] = result.Message;
                }
                else
                {
                    TempData["ErrorMessage"] = result.Message;
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Import sırasında hata oluştu: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Yeni marka ekler (AJAX endpoint)
        /// </summary>
        /// <param name="name">Marka adı</param>
        /// <returns>JSON sonuç</returns>
        [HttpPost]
        public async Task<IActionResult> AddBrand([FromBody] string name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    return Json(new { success = false, message = "Marka adı boş olamaz." });
                }

                var brand = new Brand
                {
                    Name = name.Trim(),
                    IsActive = true
                };

                var result = await _brandService.CreateBrandAsync(brand);
                if (result)
                {
                    return Json(new { 
                        success = true, 
                        message = "Marka başarıyla eklendi.",
                        brand = new { id = brand.Id, name = brand.Name }
                    });
                }
                else
                {
                    return Json(new { success = false, message = "Bu marka adı zaten kullanımda." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Hata: {ex.Message}" });
            }
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

        /// <summary>
        /// Silinmiş laptop listesine sıralama uygular
        /// </summary>
        /// <param name="query">Sıralanacak IQueryable</param>
        /// <param name="sortBy">Sıralama yapılacak alan</param>
        /// <param name="sortDirection">Sıralama yönü</param>
        /// <returns>Sıralanmış IQueryable</returns>
        private IQueryable<Laptop> ApplyDeletedLaptopsSorting(IQueryable<Laptop> query, string sortBy, string sortDirection)
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
                "silinmetarihi" => isDescending ? query.OrderByDescending(l => l.SilinmeTarihi) : query.OrderBy(l => l.SilinmeTarihi),
                "silmenedeni" => isDescending ? query.OrderByDescending(l => l.SilmeNedeni) : query.OrderBy(l => l.SilmeNedeni),
                "silenkullanici" => isDescending ? query.OrderByDescending(l => l.SilenKullanici) : query.OrderBy(l => l.SilenKullanici),
                _ => query.OrderByDescending(l => l.SilinmeTarihi) // Varsayılan sıralama Silinme Tarihi'ne göre azalan
            };
        }
    }
}
