using ITAssetManagement.Business.Services.Interfaces;
using ITAssetManagement.Core.Entities;
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

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _laptopService.DeleteLaptopAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
