using ITAssetManagement.Web.Models;
using ITAssetManagement.Web.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ITAssetManagement.Web.Extensions;

namespace ITAssetManagement.Web.Controllers
{
    /// <summary>
    /// Zimmet işlemlerini yöneten controller sınıfı
    /// </summary>
    [Authorize]
    public class AssignmentsController : Controller
    {
        private readonly IAssignmentService _assignmentService;
        private readonly IUserService _userService;
        private readonly ILaptopService _laptopService;

        /// <summary>
        /// AssignmentsController constructor
        /// </summary>
        /// <param name="assignmentService">Zimmet işlemleri servisi</param>
        /// <param name="userService">Kullanıcı işlemleri servisi</param>
        /// <param name="laptopService">Laptop işlemleri servisi</param>
        public AssignmentsController(
            IAssignmentService assignmentService,
            IUserService userService,
            ILaptopService laptopService)
        {
            _assignmentService = assignmentService;
            _userService = userService;
            _laptopService = laptopService;
        }

        /// <summary>
        /// Tüm zimmet kayıtlarını listeler
        /// </summary>
        /// <returns>Zimmet listesi view'i</returns>
        public async Task<IActionResult> Index()
        {
            var assignments = await _assignmentService.GetAllAssignmentsAsync();
            return View(assignments);
        }

        /// <summary>
        /// Belirli bir zimmet kaydının detaylarını gösterir
        /// </summary>
        /// <param name="id">Zimmet ID</param>
        /// <returns>Zimmet detay view'i</returns>
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var assignment = await _assignmentService.GetAssignmentByIdAsync(id.Value);
            if (assignment == null)
            {
                return NotFound();
            }

            return View(assignment);
        }

        /// <summary>
        /// Yeni zimmet oluşturma formunu gösterir
        /// </summary>
        /// <returns>Zimmet oluşturma view'i</returns>
        public async Task<IActionResult> Create()
        {
            var users = await _userService.GetAllUsersAsync();
            var laptops = await _laptopService.GetAvailableLaptopsAsync();
            
            // Kontroller ekleyelim
            if (users == null || !users.Any())
            {
                TempData["ErrorMessage"] = "Hiç kullanıcı bulunamadı. Lütfen önce kullanıcı ekleyin.";
                return RedirectToAction("Create", "Users");
            }
            
            if (laptops == null || !laptops.Any())
            {
                TempData["ErrorMessage"] = "Hiç uygun laptop bulunamadı. Lütfen önce laptop ekleyin.";
                return RedirectToAction("Create", "Laptops");
            }
            
            ViewBag.Users = users;
            ViewBag.Laptops = laptops;
            return View();
        }

        // POST: Assignments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Assignment assignment)
        {
            try
            {
                // Atama tarihi belirtilmemişse bugünün tarihini kullan
                if (assignment.AssignmentDate == default)
                {
                    assignment.AssignmentDate = DateTime.Now;
                }
                
                // Form'da olmayan zorunlu alanları dolduralım
                assignment.Tarih = DateTime.Now;
                assignment.IslemTipi = "Teslim";
                
                await _assignmentService.CreateAssignmentAsync(assignment);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Hata loglama
                ModelState.AddModelError(string.Empty, $"Bir hata oluştu: {ex.Message}");
                
                // Viewbag'leri yeniden doldur
                ViewBag.Users = await _userService.GetAllUsersAsync();
                ViewBag.Laptops = await _laptopService.GetAvailableLaptopsAsync();
                return View(assignment);
            }
        }

        // GET: Assignments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var assignment = await _assignmentService.GetAssignmentByIdAsync(id.Value);
            if (assignment == null)
            {
                return NotFound();
            }

            var users = await _userService.GetAllUsersAsync();
            
            // Düzenleme için, atanan laptop'ı da listelememiz gerekiyor
            var availableLaptops = await _laptopService.GetAvailableLaptopsAsync();
            var currentLaptop = await _laptopService.GetLaptopByIdAsync(assignment.LaptopId);
            
            var laptops = availableLaptops.ToList();
            if (currentLaptop != null && !laptops.Any(l => l.Id == currentLaptop.Id))
            {
                laptops.Add(currentLaptop);
            }
            
            ViewBag.Users = users ?? new List<User>();
            ViewBag.Laptops = laptops ?? new List<Laptop>();
            return View(assignment);
        }

        // POST: Assignments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Assignment assignment)
        {
            if (id != assignment.Id)
            {
                return NotFound();
            }

            try
            {
                // Entity tracking hatasını önlemek için yeni yaklaşım
                var existingAssignment = await _assignmentService.GetAssignmentByIdAsync(id);
                if (existingAssignment == null)
                {
                    return NotFound();
                }
                
                // Sadece değiştirilen alanları güncelle
                existingAssignment.UserId = assignment.UserId;
                existingAssignment.LaptopId = assignment.LaptopId;
                existingAssignment.AssignmentDate = assignment.AssignmentDate;
                existingAssignment.ReturnDate = assignment.ReturnDate;
                existingAssignment.Tarih = existingAssignment.Tarih; // Orjinal tarihi koru
                existingAssignment.IslemTipi = assignment.ReturnDate != null ? "İade" : "Teslim"; // ReturnDate'e göre IslemTipi güncelle
                
                await _assignmentService.UpdateAssignmentAsync(existingAssignment);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Bir hata oluştu: {ex.Message}");
                
                // Viewbag'leri yeniden doldur
                var users = await _userService.GetAllUsersAsync();
                var availableLaptops = await _laptopService.GetAvailableLaptopsAsync();
                var currentLaptop = await _laptopService.GetLaptopByIdAsync(assignment.LaptopId);
                
                var laptops = availableLaptops.ToList();
                if (currentLaptop != null && !laptops.Any(l => l.Id == currentLaptop.Id))
                {
                    laptops.Add(currentLaptop);
                }
                
                ViewBag.Users = users ?? new List<User>();
                ViewBag.Laptops = laptops ?? new List<Laptop>();
                return View(assignment);
            }
        }

        // GET: Assignments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var assignment = await _assignmentService.GetAssignmentByIdAsync(id.Value);
            if (assignment == null)
            {
                return NotFound();
            }

            return View(assignment);
        }

        // POST: Assignments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _assignmentService.DeleteAssignmentAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
