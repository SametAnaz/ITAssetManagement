using ITAssetManagement.Web.Services.Interfaces;
using ITAssetManagement.Web.Models;
using ITAssetManagement.Web.Data;
using ITAssetManagement.Web.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Globalization;
using OfficeOpenXml;

namespace ITAssetManagement.Web.Services
{
    /// <summary>
    /// Laptop varlıklarını yöneten servis sınıfı.
    /// ILaptopService arayüzünü implement eder.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Bu servis şu işlevleri sağlar:
    /// <list type="bullet">
    /// <item><description>Laptop envanteri yönetimi</description></item>
    /// <item><description>Zimmet durumu takibi</description></item>
    /// <item><description>Arama ve filtreleme</description></item>
    /// <item><description>Silinen laptopların yönetimi</description></item>
    /// </list>
    /// </para>
    /// </remarks>
    public class LaptopService : ILaptopService
    {
        private readonly ILaptopRepository _laptopRepository;
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// LaptopService sınıfının yeni bir instance'ını oluşturur.
        /// </summary>
        /// <param name="laptopRepository">Laptop repository'si</param>
        /// <param name="context">Veritabanı context'i</param>
        /// <remarks>
        /// Dependency injection ile gerekli bağımlılıkları alır.
        /// </remarks>
        public LaptopService(ILaptopRepository laptopRepository, ApplicationDbContext context)
        {
            _laptopRepository = laptopRepository;
            _context = context;
        }

        /// <summary>
        /// Tüm aktif laptopları sorgulanabilir şekilde getirir.
        /// </summary>
        /// <returns>Sorgulanabilir laptop listesi</returns>
        /// <remarks>
        /// Include edilmiş ilişkiler:
        /// - CurrentAssignment
        /// - User (through CurrentAssignment)
        /// </remarks>
        public IQueryable<Laptop> GetAllLaptopsQueryable()
        {
            return _context.Laptops
                .Include(l => l.CurrentAssignment)
                .ThenInclude(a => a!.User)
                .Where(l => l.IsActive);
        }

        /// <summary>
        /// Tüm aktif laptopları listeler.
        /// </summary>
        /// <returns>Laptop listesi</returns>
        /// <remarks>
        /// GetAllLaptopsQueryable metodunun execute edilmiş hali.
        /// </remarks>
        public async Task<IEnumerable<Laptop>> GetAllLaptopsAsync()
        {
            return await GetAllLaptopsQueryable().ToListAsync();
        }

        /// <summary>
        /// Zimmetlenmemiş laptopları sorgulanabilir şekilde getirir.
        /// </summary>
        /// <returns>Sorgulanabilir müsait laptop listesi</returns>
        /// <remarks>
        /// Şu koşulları sağlayan laptopları getirir:
        /// - IsActive = true
        /// - Aktif zimmeti olmayan
        /// </remarks>
        public IQueryable<Laptop> GetAvailableLaptopsQueryable()
        {
            var assignedLaptopIds = _context.Assignments
                .Where(a => a.ReturnDate == null)
                .Select(a => a.LaptopId);

            return _context.Laptops
                .Where(l => l.IsActive && !assignedLaptopIds.Contains(l.Id));
        }

        /// <summary>
        /// Zimmetlenmemiş laptopları listeler.
        /// </summary>
        /// <returns>Müsait laptop listesi</returns>
        /// <remarks>
        /// GetAvailableLaptopsQueryable metodunun execute edilmiş hali.
        /// </remarks>
        public async Task<IEnumerable<Laptop>> GetAvailableLaptopsAsync()
        {
            return await GetAvailableLaptopsQueryable().ToListAsync();
        }

        /// <summary>
        /// ID'ye göre laptop getirir.
        /// </summary>
        /// <param name="id">Laptop ID'si</param>
        /// <returns>Bulunan laptop veya null</returns>
        /// <remarks>
        /// Temel laptop bilgilerini içerir, ilişkili veriler include edilmez.
        /// </remarks>
        public async Task<Laptop?> GetLaptopByIdAsync(int id)
        {
            return await _laptopRepository.GetByIdAsync(id);
        }

        /// <summary>
        /// ID'ye göre laptop'u tüm detaylarıyla getirir.
        /// </summary>
        /// <param name="id">Laptop ID'si</param>
        /// <returns>Detaylı laptop bilgisi veya null</returns>
        /// <remarks>
        /// Include edilen ilişkiler:
        /// - Photos
        /// - Loglar
        /// - CurrentAssignment
        /// - User (through CurrentAssignment)
        /// </remarks>
        public async Task<Laptop?> GetLaptopWithDetailsAsync(int id)
        {
            return await _laptopRepository.GetLaptopWithDetailsAsync(id);
        }

        /// <summary>
        /// Yeni laptop kaydı oluşturur.
        /// </summary>
        /// <param name="laptop">Oluşturulacak laptop bilgileri</param>
        /// <returns>İşlem başarılı ise true</returns>
        /// <remarks>
        /// <para>
        /// Bu metot şu işlemleri gerçekleştirir:
        /// <list type="bullet">
        /// <item><description>Veri doğrulama</description></item>
        /// <item><description>Yeni laptop kaydı</description></item>
        /// <item><description>İlişkili koleksiyonların başlatılması</description></item>
        /// </list>
        /// </para>
        /// </remarks>
        public async Task<bool> CreateLaptopAsync(Laptop laptop)
        {
            if (laptop == null)
                return false;

            await _laptopRepository.AddAsync(laptop);
            return await _laptopRepository.SaveChangesAsync();
        }

        /// <summary>
        /// Laptop bilgilerini günceller.
        /// </summary>
        /// <param name="laptop">Güncellenecek laptop bilgileri</param>
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
        public async Task<bool> UpdateLaptopAsync(Laptop laptop)
        {
            if (laptop == null)
                return false;

            _laptopRepository.Update(laptop);
            return await _laptopRepository.SaveChangesAsync();
        }

        /// <summary>
        /// Laptop'u soft delete ile siler.
        /// </summary>
        /// <param name="id">Silinecek laptop ID'si</param>
        /// <param name="silmeNedeni">Silme nedeni açıklaması</param>
        /// <returns>İşlem başarılı ise true</returns>
        /// <remarks>
        /// <para>
        /// Bu metot şu işlemleri gerçekleştirir:
        /// <list type="bullet">
        /// <item><description>IsActive = false yapılır</description></item>
        /// <item><description>Silme tarihi ve nedeni kaydedilir</description></item>
        /// <item><description>İşlemi yapan kullanıcı bilgisi tutulur</description></item>
        /// </list>
        /// </para>
        /// </remarks>
        public async Task<bool> DeleteLaptopAsync(int id, string? silmeNedeni = null)
        {
            var laptop = await _laptopRepository.GetLaptopByIdIncludingDeletedAsync(id);
            if (laptop == null)
                return false;

            // Soft delete - laptop'ı veritabanından silmeyip sadece IsActive = false yapıyoruz
            laptop.IsActive = false;
            laptop.SilinmeTarihi = DateTime.Now;
            laptop.SilmeNedeni = !string.IsNullOrWhiteSpace(silmeNedeni) ? silmeNedeni : "Manuel silme işlemi";
            // TODO: Kullanıcı sistemi eklendikten sonra aktif edilecek
            // laptop.SilenKullanici = currentUser.UserName;
            laptop.SilenKullanici = "System Admin"; // Geçici olarak

            _laptopRepository.Update(laptop);
            return await _laptopRepository.SaveChangesAsync();
        }

        /// <summary>
        /// Silinen laptopları sorgulanabilir şekilde getirir.
        /// </summary>
        /// <returns>Sorgulanabilir silinmiş laptop listesi</returns>
        /// <remarks>
        /// IsActive = false olan kayıtları listeler.
        /// </remarks>
        public IQueryable<Laptop> GetDeletedLaptopsQueryable()
        {
            return _context.Laptops
                .Where(l => !l.IsActive);
        }

        /// <summary>
        /// Silinmiş laptopları listeler.
        /// </summary>
        /// <returns>Silinmiş laptop listesi</returns>
        /// <remarks>
        /// GetDeletedLaptopsQueryable metodunun execute edilmiş hali.
        /// </remarks>
        public async Task<IEnumerable<Laptop>> GetDeletedLaptopsAsync()
        {
            return await GetDeletedLaptopsQueryable().ToListAsync();
        }

        /// <summary>
        /// Silinmiş bir laptop'u geri yükler.
        /// </summary>
        /// <param name="id">Geri yüklenecek laptop ID'si</param>
        /// <returns>İşlem başarılı ise true</returns>
        /// <remarks>
        /// <para>
        /// Bu metot şu işlemleri gerçekleştirir:
        /// <list type="bullet">
        /// <item><description>IsActive = true yapılır</description></item>
        /// <item><description>Silme ile ilgili alanlar temizlenir</description></item>
        /// <item><description>Değişiklikler kaydedilir</description></item>
        /// </list>
        /// </para>
        /// </remarks>
        public async Task<bool> RestoreLaptopAsync(int id)
        {
            var laptop = await _laptopRepository.GetLaptopByIdIncludingDeletedAsync(id);
            if (laptop == null || laptop.IsActive)
                return false;

            laptop.IsActive = true;
            laptop.SilinmeTarihi = null;
            laptop.SilmeNedeni = null;
            laptop.SilenKullanici = null;

            _laptopRepository.Update(laptop);
            return await _laptopRepository.SaveChangesAsync();
        }

        /// <summary>
        /// Laptopları arama terimlerine göre sorgulanabilir şekilde filtreler.
        /// </summary>
        /// <param name="searchTerm">Arama terimi</param>
        /// <returns>Sorgulanabilir filtrelenmiş laptop listesi</returns>
        /// <remarks>
        /// <para>
        /// Arama şu alanlarda yapılır:
        /// <list type="bullet">
        /// <item><description>Marka</description></item>
        /// <item><description>Model</description></item>
        /// <item><description>ID</description></item>
        /// <item><description>Etiket No</description></item>
        /// </list>
        /// </para>
        /// </remarks>
        public IQueryable<Laptop> SearchLaptopsQueryable(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return GetAllLaptopsQueryable();

            searchTerm = searchTerm.ToLower();
            return _context.Laptops
                .Include(l => l.CurrentAssignment)
                .ThenInclude(a => a!.User)
                .Where(l => l.IsActive &&
                    (l.Marka.ToLower().Contains(searchTerm) ||
                     l.Model.ToLower().Contains(searchTerm) ||
                     l.Id.ToString() == searchTerm ||
                     l.EtiketNo.ToLower().Contains(searchTerm)));
        }

        /// <summary>
        /// Silinmiş laptopları arama terimlerine göre sorgulanabilir şekilde filtreler.
        /// </summary>
        /// <param name="searchTerm">Arama terimi</param>
        /// <returns>Sorgulanabilir filtrelenmiş silinmiş laptop listesi</returns>
        /// <remarks>
        /// <para>
        /// Arama şu alanlarda yapılır:
        /// <list type="bullet">
        /// <item><description>Marka</description></item>
        /// <item><description>Model</description></item>
        /// <item><description>ID</description></item>
        /// <item><description>Etiket No</description></item>
        /// <item><description>Silme Nedeni</description></item>
        /// <item><description>Silen Kullanıcı</description></item>
        /// </list>
        /// </para>
        /// </remarks>
        public IQueryable<Laptop> SearchDeletedLaptopsQueryable(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return GetDeletedLaptopsQueryable();

            searchTerm = searchTerm.ToLower();
            return _context.Laptops
                .Where(l => !l.IsActive &&
                    (l.Marka.ToLower().Contains(searchTerm) ||
                     l.Model.ToLower().Contains(searchTerm) ||
                     l.Id.ToString() == searchTerm ||
                     l.EtiketNo.ToLower().Contains(searchTerm) ||
                     (!string.IsNullOrEmpty(l.SilmeNedeni) && l.SilmeNedeni.ToLower().Contains(searchTerm)) ||
                     (!string.IsNullOrEmpty(l.SilenKullanici) && l.SilenKullanici.ToLower().Contains(searchTerm))))
                .OrderByDescending(l => l.SilinmeTarihi);
        }

        /// <summary>
        /// Laptopları arama terimlerine göre filtreler.
        /// </summary>
        /// <param name="searchTerm">Arama terimi</param>
        /// <returns>Filtrelenmiş laptop listesi</returns>
        /// <remarks>
        /// SearchLaptopsQueryable metodunun execute edilmiş hali.
        /// </remarks>
        public async Task<IEnumerable<Laptop>> SearchLaptopsAsync(string searchTerm)
        {
            return await SearchLaptopsQueryable(searchTerm).ToListAsync();
        }

        /// <summary>
        /// Laptop verilerini Excel formatında export eder
        /// </summary>
        /// <returns>Excel dosyası byte array'i</returns>
        public async Task<byte[]> ExportLaptopsToExcelAsync()
        {
            var laptops = await GetAllLaptopsQueryable().ToListAsync();
            
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            
            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Laptops");
            
            // Header
            worksheet.Cells[1, 1].Value = "Etiket No";
            worksheet.Cells[1, 2].Value = "Marka";
            worksheet.Cells[1, 3].Value = "Model";
            worksheet.Cells[1, 4].Value = "Durum";
            worksheet.Cells[1, 5].Value = "Zimmetli Kullanıcı";
            worksheet.Cells[1, 6].Value = "Kayıt Tarihi";
            
            // Header formatting
            using (var range = worksheet.Cells[1, 1, 1, 6])
            {
                range.Style.Font.Bold = true;
                range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
            }
            
            // Data rows
            for (int i = 0; i < laptops.Count; i++)
            {
                var laptop = laptops[i];
                var zimmetliKullanici = laptop.CurrentAssignment?.User?.FullName ?? "";
                
                worksheet.Cells[i + 2, 1].Value = laptop.EtiketNo;
                worksheet.Cells[i + 2, 2].Value = laptop.Marka;
                worksheet.Cells[i + 2, 3].Value = laptop.Model;
                worksheet.Cells[i + 2, 4].Value = laptop.Durum;
                worksheet.Cells[i + 2, 5].Value = zimmetliKullanici;
                worksheet.Cells[i + 2, 6].Value = laptop.KayitTarihi.ToString("dd/MM/yyyy");
            }
            
            // Auto-fit columns
            worksheet.Cells.AutoFitColumns();
            
            return package.GetAsByteArray();
        }

        /// <summary>
        /// Laptop verilerini CSV formatında export eder
        /// </summary>
        /// <returns>CSV dosyası byte array'i</returns>
        public async Task<byte[]> ExportLaptopsToCsvAsync()
        {
            var laptops = await GetAllLaptopsQueryable().ToListAsync();
            
            var csv = new StringBuilder();
            csv.AppendLine("EtiketNo,Marka,Model,Durum,ZimmetliKullanici,KayitTarihi");
            
            foreach (var laptop in laptops)
            {
                var zimmetliKullanici = laptop.CurrentAssignment?.User?.FullName ?? "";
                csv.AppendLine($"{laptop.EtiketNo},{laptop.Marka},{laptop.Model},{laptop.Durum},{zimmetliKullanici},{laptop.KayitTarihi:dd/MM/yyyy}");
            }
            
            return Encoding.UTF8.GetBytes(csv.ToString());
        }

        /// <summary>
        /// Upload edilen dosyadan laptop verilerini import eder
        /// </summary>
        /// <param name="fileBytes">Dosya byte array'i</param>
        /// <param name="fileName">Dosya adı</param>
        /// <returns>Import işlem sonucu</returns>
        public async Task<(bool Success, string Message, int ImportedCount)> ImportLaptopsFromFileAsync(byte[] fileBytes, string fileName)
        {
            try
            {
                List<string[]> dataRows;

                if (fileName.EndsWith(".xlsx") || fileName.EndsWith(".xls"))
                {
                    // EPPlus ile Excel dosyasını oku
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    
                    try
                    {
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
                            return (false, "Excel dosyasında veri bulunamadı (en az 2 satır olmalı)", 0);
                        }

                        dataRows = new List<string[]>();
                        
                        // Excel'den verileri oku
                        for (int row = 1; row <= rowCount; row++)
                        {
                            var rowData = new List<string>();
                            for (int col = 1; col <= Math.Min(colCount, 6); col++) // Max 6 kolon al
                            {
                                var cellValue = worksheet.Cells[row, col].Value?.ToString() ?? "";
                                rowData.Add(cellValue.Trim());
                            }
                            
                            // Boş olmayan satırları ekle
                            if (rowData.Count > 0 && rowData.Any(cell => !string.IsNullOrWhiteSpace(cell)))
                            {
                                dataRows.Add(rowData.ToArray());
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        return (false, $"Excel dosyası okuma hatası: {ex.Message}", 0);
                    }
                }
                else
                {
                    // CSV dosyası için
                    var content = Encoding.UTF8.GetString(fileBytes);
                    var lines = content.Split('\n', StringSplitOptions.RemoveEmptyEntries);
                    
                    dataRows = new List<string[]>();
                    foreach (var line in lines)
                    {
                        var fields = line.Split(',').Select(f => f.Trim().Trim('"')).ToArray();
                        if (fields.Length > 0 && !string.IsNullOrWhiteSpace(fields[0]))
                        {
                            dataRows.Add(fields);
                        }
                    }
                }

                if (dataRows.Count < 1)
                {
                    return (false, $"Dosya boş veya geçersiz format. Bulunan satır sayısı: {dataRows.Count}", 0);
                }

                if (dataRows.Count < 2)
                {
                    return (false, $"Dosyada sadece başlık satırı var. Data satırı bulunamadı. Toplam satır: {dataRows.Count}", 0);
                }

                var importedCount = 0;
                var skippedCount = 0;
                
                // İlk satır header, atla
                for (int i = 1; i < dataRows.Count; i++)
                {
                    var fields = dataRows[i];
                    if (fields.Length >= 4 && !string.IsNullOrWhiteSpace(fields[0]))
                    {
                        var laptop = new Laptop
                        {
                            EtiketNo = fields[0].Trim(),
                            Marka = fields[1].Trim(),
                            Model = fields[2].Trim(),
                            Durum = fields[3].Trim(),
                            KayitTarihi = DateTime.Now,
                            IsActive = true
                        };

                        // Aynı etiket no kontrol et
                        var existingLaptop = await _context.Laptops
                            .FirstOrDefaultAsync(l => l.EtiketNo == laptop.EtiketNo);
                        
                        if (existingLaptop == null)
                        {
                            _context.Laptops.Add(laptop);
                            importedCount++;
                        }
                        else
                        {
                            skippedCount++;
                        }
                    }
                    else
                    {
                        skippedCount++;
                    }
                }

                await _context.SaveChangesAsync();
                
                var message = $"{importedCount} laptop başarıyla import edildi";
                if (skippedCount > 0)
                {
                    message += $", {skippedCount} kayıt atlandı (mevcut veya geçersiz)";
                }
                
                return (true, message, importedCount);
            }
            catch (Exception ex)
            {
                return (false, $"Import sırasında hata: {ex.Message}. StackTrace: {ex.StackTrace}", 0);
            }
        }
    }
}
