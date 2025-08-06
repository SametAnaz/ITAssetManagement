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
        /// Tüm zimmet kayıtlarını sayfalı şekilde listeler
        /// </summary>
        /// <param name="searchTerm">Arama terimi</param>
        /// <param name="pageNumber">Sayfa numarası</param>
        /// <param name="pageSize">Sayfa başına kayıt sayısı</param>
        /// <param name="sortBy">Sıralama yapılacak alan</param>
        /// <param name="sortDirection">Sıralama yönü (asc/desc)</param>
        /// <returns>Sayfalanmış zimmet listesi view'i</returns>
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
            var currentSortBy = sortBy ?? "AssignmentDate";
            var currentSortDirection = sortDirection ?? "desc";
            
            ViewBag.CurrentSortBy = currentSortBy;
            ViewBag.CurrentSortDirection = currentSortDirection;

            // Sıralama için ViewBag'e değerleri gönder
            ViewBag.IdSort = currentSortBy == "Id" ? (currentSortDirection == "asc" ? "desc" : "asc") : "asc";
            ViewBag.UserSort = currentSortBy == "User" ? (currentSortDirection == "asc" ? "desc" : "asc") : "asc";
            ViewBag.LaptopSort = currentSortBy == "Laptop" ? (currentSortDirection == "asc" ? "desc" : "asc") : "asc";
            ViewBag.AssignmentDateSort = currentSortBy == "AssignmentDate" ? (currentSortDirection == "asc" ? "desc" : "asc") : "desc";
            ViewBag.ReturnDateSort = currentSortBy == "ReturnDate" ? (currentSortDirection == "asc" ? "desc" : "asc") : "asc";

            // Arama terimini ViewBag'e ekle
            ViewData["CurrentFilter"] = searchTerm;
            ViewData["CurrentSearch"] = searchTerm; // URL'lerde kullanmak için

            // Sorguyu oluştur
            var assignmentsQuery = string.IsNullOrEmpty(searchTerm) 
                ? _assignmentService.GetAllAssignmentsQueryable()
                : _assignmentService.SearchAssignmentsQueryable(searchTerm);

            // Sıralama işlemi
            assignmentsQuery = ApplyAssignmentsSorting(assignmentsQuery, currentSortBy, currentSortDirection);

            // Sayfalama yap
            return View(await PaginatedList<Assignment>.CreateAsync(
                assignmentsQuery,
                currentPageNumber,
                currentPageSize));
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

        /// <summary>
        /// Zimmet verilerini Excel formatında export eder
        /// </summary>
        /// <returns>Excel dosyası</returns>
        public async Task<IActionResult> ExportToExcel()
        {
            try
            {
                var excelData = await _assignmentService.ExportAssignmentsToExcelAsync();
                var fileName = $"Assignments_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
                return File(excelData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Export sırasında hata oluştu: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        /// <summary>
        /// Zimmet verilerini CSV formatında export eder
        /// </summary>
        /// <returns>CSV dosyası</returns>
        public async Task<IActionResult> ExportToCsv()
        {
            try
            {
                var csvData = await _assignmentService.ExportAssignmentsToCsvAsync();
                var fileName = $"Assignments_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
                return File(csvData, "text/csv", fileName);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Export sırasında hata oluştu: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        /// <summary>
        /// Zimmet verilerini dosyadan import eder
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

                var result = await _assignmentService.ImportAssignmentsFromFileAsync(fileBytes, file.FileName);
                
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
        /// Kullanıcı detay bilgilerini AJAX ile döndürür
        /// </summary>
        /// <param name="userId">Kullanıcı ID</param>
        /// <returns>JSON formatında kullanıcı detayları</returns>
        [HttpGet]
        public async Task<IActionResult> GetUserInfo(int userId)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(userId);
                if (user == null)
                {
                    return NotFound(new { success = false, message = "Kullanıcı bulunamadı." });
                }

                // Kullanıcının zimmet sayısını hesapla
                var allAssignments = _assignmentService.GetAllAssignmentsQueryable()
                    .Where(a => a.UserId == userId);
                var activeAssignments = allAssignments.Where(a => a.ReturnDate == null).Count();
                var totalAssignments = allAssignments.Count();
                var lastAssignment = allAssignments.OrderByDescending(a => a.AssignmentDate).FirstOrDefault();

                var userInfo = new
                {
                    success = true,
                    user = new
                    {
                        id = user.Id,
                        fullName = user.FullName,
                        email = user.Email,
                        phone = user.Phone,
                        department = user.Department,
                        position = user.Position,
                        activeAssignmentsCount = activeAssignments,
                        totalAssignmentsCount = totalAssignments,
                        lastAssignmentDate = lastAssignment?.AssignmentDate.ToString("dd.MM.yyyy") ?? "Hiç zimmet yok"
                    }
                };

                return Json(userInfo);
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = "Kullanıcı bilgileri alınırken hata oluştu: " + ex.Message });
            }
        }

        /// <summary>
        /// Laptop detay bilgilerini AJAX ile döndürür
        /// </summary>
        /// <param name="laptopId">Laptop ID</param>
        /// <returns>JSON formatında laptop detayları</returns>
        [HttpGet]
        public async Task<IActionResult> GetLaptopInfo(int laptopId)
        {
            try
            {
                var laptop = await _laptopService.GetLaptopWithDetailsAsync(laptopId);
                if (laptop == null)
                {
                    return NotFound(new { success = false, message = "Laptop bulunamadı." });
                }

                // Laptop'ın zimmet geçmişini hesapla
                var allAssignments = _assignmentService.GetAllAssignmentsQueryable()
                    .Where(a => a.LaptopId == laptopId);
                var activeAssignment = allAssignments.FirstOrDefault(a => a.ReturnDate == null);
                var totalAssignments = allAssignments.Count();
                var lastAssignment = allAssignments.OrderByDescending(a => a.AssignmentDate).FirstOrDefault();

                var laptopInfo = new
                {
                    success = true,
                    laptop = new
                    {
                        id = laptop.Id,
                        etiketNo = laptop.EtiketNo,
                        marka = laptop.Marka,
                        model = laptop.Model,
                        ozellikler = laptop.Ozellikler,
                        durum = laptop.Durum,
                        notes = laptop.Notes,
                        isActive = laptop.IsActive,
                        displayName = laptop.DisplayName,
                        isAssigned = activeAssignment != null,
                        currentAssignedUser = activeAssignment?.User?.FullName ?? "Kimseye zimmetli değil",
                        totalAssignmentsCount = totalAssignments,
                        lastAssignmentDate = lastAssignment?.AssignmentDate.ToString("dd.MM.yyyy") ?? "Hiç zimmet yok"
                    }
                };

                return Json(laptopInfo);
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = "Laptop bilgileri alınırken hata oluştu: " + ex.Message });
            }
        }

        /// <summary>
        /// Assignment listesine sıralama uygular
        /// </summary>
        /// <param name="query">Sıralanacak IQueryable</param>
        /// <param name="sortBy">Sıralama yapılacak alan</param>
        /// <param name="sortDirection">Sıralama yönü</param>
        /// <returns>Sıralanmış IQueryable</returns>
        private IQueryable<Assignment> ApplyAssignmentsSorting(IQueryable<Assignment> query, string sortBy, string sortDirection)
        {
            var isDescending = sortDirection.ToLower() == "desc";

            return sortBy.ToLower() switch
            {
                "id" => isDescending ? query.OrderByDescending(a => a.Id) : query.OrderBy(a => a.Id),
                "user" => isDescending ? query.OrderByDescending(a => a.User!.FullName) : query.OrderBy(a => a.User!.FullName),
                "laptop" => isDescending ? query.OrderByDescending(a => a.Laptop!.Marka + " " + a.Laptop.Model) : query.OrderBy(a => a.Laptop!.Marka + " " + a.Laptop.Model),
                "assignmentdate" => isDescending ? query.OrderByDescending(a => a.AssignmentDate) : query.OrderBy(a => a.AssignmentDate),
                "returndate" => isDescending ? query.OrderByDescending(a => a.ReturnDate) : query.OrderBy(a => a.ReturnDate),
                _ => query.OrderByDescending(a => a.AssignmentDate) // Varsayılan sıralama Atama Tarihi'ne göre azalan
            };
        }
    }
}
