using ITAssetManagement.Web.Services.Interfaces;
using ITAssetManagement.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace ITAssetManagement.Web.Controllers
{
    public class LaptopsController : Controller
    {
        private readonly ILaptopService _laptopService;

        public LaptopsController(ILaptopService laptopService)
        {
            _laptopService = laptopService;
        }

        public async Task<IActionResult> Index()
        {
            var laptops = await _laptopService.GetAllLaptopsAsync();
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
                await _laptopService.CreateLaptopAsync(laptop);
                TempData["SuccessMessage"] = "Laptop başarıyla eklendi.";
                return RedirectToAction(nameof(Index));
            }
            return View(laptop);
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
    }
}
