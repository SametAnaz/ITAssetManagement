using ITAssetManagement.Web.Services.Interfaces;
using ITAssetManagement.Web.Models;
using Microsoft.AspNetCore.Mvc;
using ITAssetManagement.Web.Extensions;
using ITAssetManagement.Web.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace ITAssetManagement.Web.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private readonly IUserService _userService;
        private readonly ApplicationDbContext _context;

        public UsersController(IUserService userService, ApplicationDbContext context)
        {
            _userService = userService;
            _context = context;
        }

        // GET: Users
        public async Task<IActionResult> Index(string searchTerm, int? pageNumber, int? pageSize)
        {
            // Sayfa başına kayıt sayısı seçenekleri
            var pageSizeOptions = new List<int> { 5, 10, 25, 50 };
            ViewBag.PageSizeOptions = pageSizeOptions;
            
            // Seçilen sayfa başına kayıt sayısı veya varsayılan değer (10)
            int currentPageSize = pageSize ?? 10;
            ViewBag.CurrentPageSize = currentPageSize;

            // Sayfa numarası veya varsayılan değer (1)
            int currentPageNumber = pageNumber ?? 1;

            // Arama terimini ViewBag'e ekle
            ViewData["CurrentFilter"] = searchTerm;
            ViewData["CurrentSearch"] = searchTerm; // URL'lerde kullanmak için

            // Sorguyu oluştur
            var usersQuery = _userService.SearchUsersQueryable(searchTerm ?? string.Empty);

            return View(await PaginatedList<User>.CreateAsync(usersQuery, currentPageNumber, currentPageSize));
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(User user)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.CreateUserAsync(user);
                if (result)
                {
                    TempData["SuccessMessage"] = "Kullanıcı başarıyla oluşturuldu.";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("Email", "Bu email adresi zaten kullanımda.");
                }
            }
            return View(user);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var result = await _userService.UpdateUserAsync(user);
                if (result)
                {
                    TempData["SuccessMessage"] = "Kullanıcı başarıyla güncellendi.";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["ErrorMessage"] = "Kullanıcı güncellenirken bir hata oluştu.";
                }
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _userService.DeleteUserAsync(id);
            if (result)
            {
                TempData["SuccessMessage"] = "Kullanıcı başarıyla silindi.";
            }
            else
            {
                TempData["ErrorMessage"] = "Kullanıcı silinirken bir hata oluştu.";
            }
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Kullanıcı verilerini Excel formatında export eder
        /// </summary>
        /// <returns>Excel dosyası</returns>
        public async Task<IActionResult> ExportToExcel()
        {
            try
            {
                var excelData = await _userService.ExportUsersToExcelAsync();
                var fileName = $"Users_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
                return File(excelData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Export sırasında hata oluştu: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        /// <summary>
        /// Kullanıcı verilerini CSV formatında export eder
        /// </summary>
        /// <returns>CSV dosyası</returns>
        public async Task<IActionResult> ExportToCsv()
        {
            try
            {
                var csvData = await _userService.ExportUsersToCsvAsync();
                var fileName = $"Users_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
                return File(csvData, "text/csv", fileName);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Export sırasında hata oluştu: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        /// <summary>
        /// Kullanıcı verilerini dosyadan import eder
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

                var result = await _userService.ImportUsersFromFileAsync(fileBytes, file.FileName);
                
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
    }
}
