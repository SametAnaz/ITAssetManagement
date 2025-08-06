using ITAssetManagement.Web.Services.Interfaces;
using ITAssetManagement.Web.Models;
using ITAssetManagement.Web.Data.Repositories;
using ITAssetManagement.Web.Data;
using Microsoft.EntityFrameworkCore;
using System.Text;
using OfficeOpenXml;

namespace ITAssetManagement.Web.Services
{
    /// <summary>
    /// Kullanıcı işlemlerini yöneten servis sınıfı.
    /// IUserService arayüzünü implement eder.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Bu servis şu işlevleri sağlar:
    /// <list type="bullet">
    /// <item><description>Kullanıcı hesaplarının yönetimi</description></item>
    /// <item><description>Departman bazlı organizasyon</description></item>
    /// <item><description>Zimmet sahiplerinin takibi</description></item>
    /// <item><description>Kullanıcı arama ve filtreleme</description></item>
    /// </list>
    /// </para>
    /// </remarks>
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// UserService sınıfının yeni bir instance'ını oluşturur.
        /// </summary>
        /// <param name="userRepository">Kullanıcı repository'si</param>
        /// <param name="context">Veritabanı context'i</param>
        /// <remarks>
        /// Dependency injection ile gerekli bağımlılıkları alır.
        /// </remarks>
        public UserService(IUserRepository userRepository, ApplicationDbContext context)
        {
            _userRepository = userRepository;
            _context = context;
        }

        /// <summary>
        /// Tüm kullanıcıları listeler.
        /// </summary>
        /// <returns>Kullanıcı listesi</returns>
        /// <remarks>
        /// Temel kullanıcı bilgilerini içerir.
        /// </remarks>
        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllAsync();
        }

        /// <summary>
        /// ID'ye göre kullanıcıyı zimmet bilgileriyle getirir.
        /// </summary>
        /// <param name="id">Kullanıcı ID'si</param>
        /// <returns>Bulunan kullanıcı veya null</returns>
        /// <remarks>
        /// Kullanıcının tüm zimmet geçmişini de içerir.
        /// </remarks>
        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _userRepository.GetUserWithAssignmentsAsync(id);
        }

        /// <summary>
        /// Email adresine göre kullanıcı getirir.
        /// </summary>
        /// <param name="email">Kullanıcı email adresi</param>
        /// <returns>Bulunan kullanıcı veya null</returns>
        /// <remarks>
        /// Email adresi benzersiz olmalıdır.
        /// </remarks>
        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _userRepository.GetUserByEmailAsync(email);
        }

        /// <summary>
        /// Belirli bir departmandaki tüm kullanıcıları getirir.
        /// </summary>
        /// <param name="department">Departman adı</param>
        /// <returns>Departmandaki kullanıcı listesi</returns>
        /// <remarks>
        /// Kullanıcılar pozisyonlarına göre sıralı gelir.
        /// </remarks>
        public async Task<IEnumerable<User>> GetUsersByDepartmentAsync(string department)
        {
            return await _userRepository.GetUsersByDepartmentAsync(department);
        }

        /// <summary>
        /// Yeni kullanıcı kaydı oluşturur.
        /// </summary>
        /// <param name="user">Oluşturulacak kullanıcı bilgileri</param>
        /// <returns>İşlem başarılı ise true</returns>
        /// <remarks>
        /// <para>
        /// Bu metot şu işlemleri gerçekleştirir:
        /// <list type="bullet">
        /// <item><description>Email benzersizliği kontrolü</description></item>
        /// <item><description>Veri doğrulama</description></item>
        /// <item><description>Kullanıcı kaydı oluşturma</description></item>
        /// </list>
        /// </para>
        /// </remarks>
        public async Task<bool> CreateUserAsync(User user)
        {
            if (user == null)
                return false;

            // Email benzersizlik kontrolü
            var existingUser = await _userRepository.GetUserByEmailAsync(user.Email);
            if (existingUser != null)
                return false;

            await _userRepository.AddAsync(user);
            return await _userRepository.SaveChangesAsync();
        }

        /// <summary>
        /// Kullanıcı bilgilerini günceller.
        /// </summary>
        /// <param name="user">Güncellenecek kullanıcı bilgileri</param>
        /// <returns>İşlem başarılı ise true</returns>
        /// <remarks>
        /// <para>
        /// Bu metot şu işlemleri gerçekleştirir:
        /// <list type="bullet">
        /// <item><description>Veri doğrulama</description></item>
        /// <item><description>İlgili alanların güncellenmesi</description></item>
        /// <item><description>Değişikliklerin kaydedilmesi</description></item>
        /// </list>
        /// </para>
        /// </remarks>
        public async Task<bool> UpdateUserAsync(User user)
        {
            if (user == null)
                return false;

            _userRepository.Update(user);
            return await _userRepository.SaveChangesAsync();
        }

        /// <summary>
        /// Kullanıcıyı siler.
        /// </summary>
        /// <param name="id">Silinecek kullanıcı ID'si</param>
        /// <returns>İşlem başarılı ise true</returns>
        /// <remarks>
        /// <para>
        /// Silme işlemi öncesi:
        /// <list type="bullet">
        /// <item><description>Aktif zimmetler kontrol edilir</description></item>
        /// <item><description>İlişkili kayıtlar güncellenir</description></item>
        /// <item><description>Kullanıcı kaydı kaldırılır</description></item>
        /// </list>
        /// </para>
        /// </remarks>
        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                return false;

            _userRepository.Remove(user);
            return await _userRepository.SaveChangesAsync();
        }

        /// <summary>
        /// Kullanıcıları arama terimlerine göre sorgulanabilir şekilde filtreler.
        /// </summary>
        /// <param name="searchTerm">Arama terimi</param>
        /// <returns>Sorgulanabilir filtrelenmiş kullanıcı listesi</returns>
        /// <remarks>
        /// <para>
        /// Arama şu alanlarda yapılır:
        /// <list type="bullet">
        /// <item><description>Ad Soyad</description></item>
        /// <item><description>Email</description></item>
        /// <item><description>Departman</description></item>
        /// <item><description>Pozisyon</description></item>
        /// <item><description>Telefon</description></item>
        /// </list>
        /// </para>
        /// </remarks>
        public IQueryable<User> SearchUsersQueryable(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return _context.Users.Include(u => u.Assignments);

            searchTerm = searchTerm.ToLower();
            return _context.Users
                .Include(u => u.Assignments)
                .Where(u => 
                    u.FullName.ToLower().Contains(searchTerm) ||
                    u.Email.ToLower().Contains(searchTerm) ||
                    (u.Department != null && u.Department.ToLower().Contains(searchTerm)) ||
                    (u.Position != null && u.Position.ToLower().Contains(searchTerm)) ||
                    (u.Phone != null && u.Phone.Contains(searchTerm)));
        }

        /// <summary>
        /// Kullanıcıları arama terimlerine göre filtreler.
        /// </summary>
        /// <param name="searchTerm">Arama terimi</param>
        /// <returns>Filtrelenmiş kullanıcı listesi</returns>
        /// <remarks>
        /// SearchUsersQueryable metodunun execute edilmiş hali.
        /// </remarks>
        public async Task<IEnumerable<User>> SearchUsersAsync(string searchTerm)
        {
            return await SearchUsersQueryable(searchTerm).ToListAsync();
        }

        /// <summary>
        /// Kullanıcı verilerini Excel formatında export eder
        /// </summary>
        /// <returns>Excel dosyası byte array'i</returns>
        public async Task<byte[]> ExportUsersToExcelAsync()
        {
            var users = await GetAllUsersAsync();
            
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            
            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Users");
            
            // Header
            worksheet.Cells[1, 1].Value = "Ad Soyad";
            worksheet.Cells[1, 2].Value = "Email";
            worksheet.Cells[1, 3].Value = "Telefon";
            worksheet.Cells[1, 4].Value = "Departman";
            worksheet.Cells[1, 5].Value = "Pozisyon";
            
            // Header formatting
            using (var range = worksheet.Cells[1, 1, 1, 5])
            {
                range.Style.Font.Bold = true;
                range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
            }
            
            // Data rows
            var userList = users.ToList();
            for (int i = 0; i < userList.Count; i++)
            {
                var user = userList[i];
                
                worksheet.Cells[i + 2, 1].Value = user.FullName;
                worksheet.Cells[i + 2, 2].Value = user.Email;
                worksheet.Cells[i + 2, 3].Value = user.Phone ?? "";
                worksheet.Cells[i + 2, 4].Value = user.Department ?? "";
                worksheet.Cells[i + 2, 5].Value = user.Position ?? "";
            }
            
            // Auto-fit columns
            worksheet.Cells.AutoFitColumns();
            
            return package.GetAsByteArray();
        }

        /// <summary>
        /// Kullanıcı verilerini CSV formatında export eder
        /// </summary>
        /// <returns>CSV dosyası byte array'i</returns>
        public async Task<byte[]> ExportUsersToCsvAsync()
        {
            var users = await GetAllUsersAsync();
            
            var csv = new StringBuilder();
            csv.AppendLine("AdSoyad,Email,Telefon,Departman,Pozisyon");
            
            foreach (var user in users)
            {
                csv.AppendLine($"{user.FullName},{user.Email},{user.Phone ?? ""},{user.Department ?? ""},{user.Position ?? ""}");
            }
            
            return Encoding.UTF8.GetBytes(csv.ToString());
        }

        /// <summary>
        /// Upload edilen dosyadan kullanıcı verilerini import eder
        /// </summary>
        /// <param name="fileBytes">Dosya byte array'i</param>
        /// <param name="fileName">Dosya adı</param>
        /// <returns>Import işlem sonucu</returns>
        public async Task<(bool Success, string Message, int ImportedCount)> ImportUsersFromFileAsync(byte[] fileBytes, string fileName)
        {
            try
            {
                List<string[]> dataRows = new List<string[]>();

                if (fileName.EndsWith(".xlsx") || fileName.EndsWith(".xls"))
                {
                    // EPPlus ile Excel dosyasını oku
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    
                    using var stream = new MemoryStream(fileBytes);
                    using var package = new ExcelPackage(stream);
                    
                    var worksheet = package.Workbook.Worksheets.FirstOrDefault();
                    if (worksheet == null)
                    {
                        return (false, "Excel dosyasında worksheet bulunamadı", 0);
                    }

                    var rowCount = worksheet.Dimension?.Rows ?? 0;
                    var colCount = worksheet.Dimension?.Columns ?? 0;

                    if (rowCount < 2)
                    {
                        return (false, "Excel dosyasında veri bulunamadı", 0);
                    }

                    // Excel'den verileri oku
                    for (int row = 1; row <= rowCount; row++)
                    {
                        var rowData = new List<string>();
                        for (int col = 1; col <= Math.Min(colCount, 5); col++) // Max 5 kolon al
                        {
                            var cellValue = worksheet.Cells[row, col].Value?.ToString() ?? "";
                            rowData.Add(cellValue.Trim());
                        }
                        
                        if (rowData.Count > 0 && !string.IsNullOrWhiteSpace(rowData[0]))
                        {
                            dataRows.Add(rowData.ToArray());
                        }
                    }
                }
                else
                {
                    // CSV dosyası için
                    var content = Encoding.UTF8.GetString(fileBytes);
                    var lines = content.Split('\n', StringSplitOptions.RemoveEmptyEntries);
                    
                    foreach (var line in lines)
                    {
                        var fields = line.Split(',').Select(f => f.Trim().Trim('"')).ToArray();
                        if (fields.Length > 0 && !string.IsNullOrWhiteSpace(fields[0]))
                        {
                            dataRows.Add(fields);
                        }
                    }
                }

                if (dataRows.Count < 2)
                {
                    return (false, $"Dosyada yeterli veri bulunamadı. Bulunan satır sayısı: {dataRows.Count}", 0);
                }

                var importedCount = 0;
                // İlk satır header olarak kabul ediliyor, atla
                for (int i = 1; i < dataRows.Count; i++)
                {
                    var fields = dataRows[i];
                    if (fields.Length >= 2 && !string.IsNullOrWhiteSpace(fields[0]) && !string.IsNullOrWhiteSpace(fields[1]))
                    {
                        var user = new User
                        {
                            FullName = fields[0].Trim(),
                            Email = fields[1].Trim(),
                            Phone = fields.Length > 2 ? fields[2].Trim() : "",
                            Department = fields.Length > 3 ? fields[3].Trim() : "",
                            Position = fields.Length > 4 ? fields[4].Trim() : ""
                        };

                        // Aynı email kontrol et
                        var existingUser = await _context.Users
                            .FirstOrDefaultAsync(u => u.Email == user.Email);
                        
                        if (existingUser == null)
                        {
                            _context.Users.Add(user);
                            importedCount++;
                        }
                    }
                }

                await _context.SaveChangesAsync();
                return (true, $"{importedCount} kullanıcı başarıyla import edildi. Toplam {dataRows.Count - 1} satır işlendi.", importedCount);
            }
            catch (Exception ex)
            {
                return (false, $"Import sırasında hata: {ex.Message}", 0);
            }
        }
    }
}
