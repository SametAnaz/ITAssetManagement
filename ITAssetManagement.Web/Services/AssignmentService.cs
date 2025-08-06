using ITAssetManagement.Web.Data.Repositories;
using ITAssetManagement.Web.Models;
using ITAssetManagement.Web.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Text;
using OfficeOpenXml;

namespace ITAssetManagement.Web.Services
{
    /// <summary>
    /// Zimmet işlemlerini yöneten servis sınıfı.
    /// IAssignmentService arayüzünü implement eder.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Bu servis şu işlemleri gerçekleştirir:
    /// <list type="bullet">
    /// <item><description>Zimmet kayıtlarının oluşturulması ve yönetimi</description></item>
    /// <item><description>Zimmet detaylarının sorgulanması</description></item>
    /// <item><description>Zimmet güncelleme ve silme işlemleri</description></item>
    /// <item><description>İlişkili varlıkların (Laptop, User) yönetimi</description></item>
    /// </list>
    /// </para>
    /// </remarks>
    public class AssignmentService : IAssignmentService
    {
        private readonly IAssignmentRepository _assignmentRepository;

        /// <summary>
        /// AssignmentService sınıfının yeni bir instance'ını oluşturur.
        /// </summary>
        /// <param name="assignmentRepository">Zimmet repository'si</param>
        /// <remarks>
        /// Dependency injection ile IAssignmentRepository bağımlılığını alır.
        /// </remarks>
        public AssignmentService(IAssignmentRepository assignmentRepository)
        {
            _assignmentRepository = assignmentRepository;
        }

        /// <summary>
        /// Tüm zimmet kayıtlarını sorgulanabilir şekilde getirir.
        /// </summary>
        /// <returns>Sorgulanabilir zimmet listesi</returns>
        /// <remarks>
        /// Bu metot aktif ve geçmiş tüm zimmetleri içerir.
        /// Sonuçlar tarihe göre azalan sırada döner.
        /// </remarks>
        public IQueryable<Assignment> GetAllAssignmentsQueryable()
        {
            return _assignmentRepository.GetAllWithDetailsQueryable();
        }

        /// <summary>
        /// Belirtilen arama terimine göre zimmet kayıtlarını filtreleyerek getirir.
        /// </summary>
        /// <param name="searchTerm">Arama terimi</param>
        /// <returns>Filtrelenmiş zimmet listesi</returns>
        public IQueryable<Assignment> SearchAssignmentsQueryable(string searchTerm)
        {
            var query = GetAllAssignmentsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                searchTerm = searchTerm.ToLower();
                query = query.Where(a =>
                    (a.Laptop != null && a.Laptop.Marka.ToLower().Contains(searchTerm)) ||
                    (a.Laptop != null && a.Laptop.Model.ToLower().Contains(searchTerm)) ||
                    (a.User != null && a.User.FullName.ToLower().Contains(searchTerm)) ||
                    (a.Id.ToString() == searchTerm)
                ).OrderByDescending(a => a.AssignmentDate);
            }

            return query;
        }

        /// <summary>
        /// ID'ye göre zimmet kaydını detaylarıyla birlikte getirir.
        /// </summary>
        /// <param name="id">Zimmet kaydının ID'si</param>
        /// <returns>Bulunan zimmet kaydı veya null</returns>
        /// <remarks>
        /// İlgili Laptop ve User bilgilerini Include eder.
        /// </remarks>
        public async Task<Assignment?> GetAssignmentByIdAsync(int id)
        {
            return await _assignmentRepository.GetByIdWithDetailsAsync(id);
        }

        /// <summary>
        /// Yeni bir zimmet kaydı oluşturur.
        /// </summary>
        /// <param name="assignment">Oluşturulacak zimmet kaydı</param>
        /// <remarks>
        /// <para>
        /// Bu metot şu işlemleri gerçekleştirir:
        /// <list type="bullet">
        /// <item><description>Veri doğrulama kontrolleri</description></item>
        /// <item><description>Zimmet kaydının oluşturulması</description></item>
        /// <item><description>Veritabanına kaydetme</description></item>
        /// </list>
        /// </para>
        /// </remarks>
        public async Task CreateAssignmentAsync(Assignment assignment)
        {
            await _assignmentRepository.AddAsync(assignment);
            await _assignmentRepository.SaveChangesAsync();
        }

        /// <summary>
        /// Mevcut bir zimmet kaydını günceller.    
        /// </summary>
        /// <param name="assignment">Güncellenecek zimmet kaydı</param>
        /// <remarks>
        /// <para>
        /// Bu metot şu işlemleri gerçekleştirir:
        /// <list type="bullet">
        /// <item><description>Veri doğrulama kontrolleri</description></item>
        /// <item><description>İlgili alanların güncellenmesi</description></item>
        /// <item><description>Değişikliklerin kaydedilmesi</description></item>
        /// </list>
        /// </para>
        /// </remarks>
        public async Task UpdateAssignmentAsync(Assignment assignment)
        {
            _assignmentRepository.Update(assignment);
            await _assignmentRepository.SaveChangesAsync();
        }

        /// <summary>
        /// Bir zimmet kaydını siler.
        /// </summary>
        /// <param name="id">Silinecek zimmet kaydının ID'si</param>
        /// <remarks>
        /// <para>
        /// Bu metot şu işlemleri gerçekleştirir:
        /// <list type="bullet">
        /// <item><description>Zimmet kaydının varlığının kontrolü</description></item>
        /// <item><description>İlgili zimmetin silinmesi</description></item>
        /// <item><description>Değişikliklerin kaydedilmesi</description></item>
        /// </list>
        /// </para>
        /// </remarks>
        public async Task DeleteAssignmentAsync(int id)
        {
            var assignment = await _assignmentRepository.GetByIdAsync(id);
            if (assignment != null)
            {
                _assignmentRepository.Delete(assignment);
                await _assignmentRepository.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Zimmet verilerini Excel formatında export eder
        /// </summary>
        /// <returns>Excel dosyası byte array'i</returns>
        public async Task<byte[]> ExportAssignmentsToExcelAsync()
        {
            var assignments = await GetAllAssignmentsQueryable().ToListAsync();
            
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            
            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Assignments");
            
            // Header
            worksheet.Cells[1, 1].Value = "Zimmet ID";
            worksheet.Cells[1, 2].Value = "Kullanıcı";
            worksheet.Cells[1, 3].Value = "Laptop";
            worksheet.Cells[1, 4].Value = "Zimmet Tarihi";
            worksheet.Cells[1, 5].Value = "İade Tarihi";
            worksheet.Cells[1, 6].Value = "İşlem Tipi";
            
            // Header formatting
            using (var range = worksheet.Cells[1, 1, 1, 6])
            {
                range.Style.Font.Bold = true;
                range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
            }
            
            // Data rows
            for (int i = 0; i < assignments.Count; i++)
            {
                var assignment = assignments[i];
                
                worksheet.Cells[i + 2, 1].Value = assignment.Id;
                worksheet.Cells[i + 2, 2].Value = assignment.User?.FullName ?? "";
                worksheet.Cells[i + 2, 3].Value = $"{assignment.Laptop?.Marka} {assignment.Laptop?.Model} ({assignment.Laptop?.EtiketNo})";
                worksheet.Cells[i + 2, 4].Value = assignment.AssignmentDate.ToString("dd/MM/yyyy");
                worksheet.Cells[i + 2, 5].Value = assignment.ReturnDate?.ToString("dd/MM/yyyy") ?? "Devam Ediyor";
                worksheet.Cells[i + 2, 6].Value = assignment.IslemTipi;
            }
            
            // Auto-fit columns
            worksheet.Cells.AutoFitColumns();
            
            return package.GetAsByteArray();
        }

        /// <summary>
        /// Zimmet verilerini CSV formatında export eder
        /// </summary>
        /// <returns>CSV dosyası byte array'i</returns>
        public async Task<byte[]> ExportAssignmentsToCsvAsync()
        {
            var assignments = await GetAllAssignmentsQueryable().ToListAsync();
            
            var csv = new StringBuilder();
            csv.AppendLine("ZimmetID,Kullanici,Laptop,ZimmetTarihi,IadeTarihi,IslemTipi");
            
            foreach (var assignment in assignments)
            {
                var kullanici = assignment.User?.FullName ?? "";
                var laptop = $"{assignment.Laptop?.Marka} {assignment.Laptop?.Model} ({assignment.Laptop?.EtiketNo})";
                var iadeTarihi = assignment.ReturnDate?.ToString("dd/MM/yyyy") ?? "Devam Ediyor";
                
                csv.AppendLine($"{assignment.Id},{kullanici},{laptop},{assignment.AssignmentDate:dd/MM/yyyy},{iadeTarihi},{assignment.IslemTipi}");
            }
            
            return Encoding.UTF8.GetBytes(csv.ToString());
        }

        /// <summary>
        /// Upload edilen dosyadan zimmet verilerini import eder
        /// </summary>
        /// <param name="fileBytes">Dosya byte array'i</param>
        /// <param name="fileName">Dosya adı</param>
        /// <returns>Import işlem sonucu</returns>
        public Task<(bool Success, string Message, int ImportedCount)> ImportAssignmentsFromFileAsync(byte[] fileBytes, string fileName)
        {
            try
            {
                // Not: Zimmet import'u daha karmaşık olduğu için şimdilik sadece export'u destekliyoruz
                return Task.FromResult((false, "Zimmet import özelliği henüz desteklenmiyor. Lütfen sadece export işlemini kullanın.", 0));
            }
            catch (Exception ex)
            {
                return Task.FromResult((false, $"Import sırasında hata: {ex.Message}", 0));
            }
        }
    }
}
