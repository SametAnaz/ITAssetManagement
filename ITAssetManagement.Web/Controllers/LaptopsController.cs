using ITAssetManagement.Web.Services.Interfaces;
using ITAssetManagement.Web.Services;
using ITAssetManagement.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace ITAssetManagement.Web.Controllers
{
    public class LaptopsController : Controller
    {
        private readonly ILaptopService _laptopService;
        private readonly IBarcodeService _barcodeService;

        public LaptopsController(ILaptopService laptopService, IBarcodeService barcodeService)
        {
            _laptopService = laptopService;
            _barcodeService = barcodeService;
        }

        public async Task<IActionResult> Index(string searchTerm)
        {
            var laptops = string.IsNullOrEmpty(searchTerm)
                ? await _laptopService.GetAllLaptopsAsync()
                : await _laptopService.SearchLaptopsAsync(searchTerm);
            ViewData["CurrentFilter"] = searchTerm;
            return View(laptops);
        }

        public async Task<IActionResult> Details(int id)
        {
            var laptop = await _laptopService.GetLaptopWithDetailsAsync(id);
            if (laptop == null)
            {
                return NotFound();
            }
            return View(laptop);
        }

        public IActionResult Create()
        {
            return View();
        }

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
                    return View(laptop);
                }

                // EtiketNo'nun benzersiz olup olmadığını kontrol et
                var existingLaptop = await _laptopService.GetAllLaptopsAsync();
                if (existingLaptop.Any(l => l.EtiketNo.Equals(laptop.EtiketNo, StringComparison.OrdinalIgnoreCase)))
                {
                    ModelState.AddModelError("EtiketNo", "Bu etiket numarası zaten kullanımda.");
                    return View(laptop);
                }
                
                await _laptopService.CreateLaptopAsync(laptop);
                TempData["SuccessMessage"] = "Laptop başarıyla eklendi.";
                return RedirectToAction(nameof(Index));
            }
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
                await _laptopService.UpdateLaptopAsync(laptop);
                TempData["SuccessMessage"] = "Laptop başarıyla güncellendi.";
                return RedirectToAction(nameof(Index));
            }
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

        public async Task<IActionResult> DeletedLaptops()
        {
            var deletedLaptops = await _laptopService.GetDeletedLaptopsAsync();
            return View(deletedLaptops);
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
    }
}
