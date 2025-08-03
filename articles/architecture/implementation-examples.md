# Detaylı Uygulama Örnekleri

## 1. E-posta Bildirim Sistemi Implementasyonu

### E-posta Servisi
```csharp
public class EmailService : IEmailService
{
    private readonly EmailConfiguration _config;
    private readonly ILogger<EmailService> _logger;
    
    public async Task SendAssignmentNotification(Assignment assignment)
    {
        try
        {
            var template = await GetEmailTemplate("AssignmentNotification");
            var emailContent = PrepareEmailContent(template, assignment);
            
            await SendEmailAsync(new EmailMessage
            {
                To = assignment.User.Email,
                Subject = "Yeni Zimmet Bildirimi",
                Content = emailContent,
                Attachments = new[] { assignment.QRCodeUrl }
            });
            
            await LogEmailSuccess(assignment.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Zimmet bildirimi gönderilemedi");
            await LogEmailError(assignment.Id, ex.Message);
            throw;
        }
    }
}
```

## 2. QR Kod İşlemleri

### QR Kod Servisi
```csharp
public class QRCodeService : IQRCodeService
{
    private readonly IConfiguration _config;
    
    public async Task<QRCodeResult> Generate(Assignment assignment)
    {
        var data = new
        {
            AssignmentId = assignment.Id,
            UserName = assignment.User.Username,
            LaptopSerial = assignment.Laptop.SerialNumber,
            ExpiryDate = assignment.ReturnDate
        };
        
        var jsonData = JsonSerializer.Serialize(data);
        var qrCodeImage = QRCodeGenerator.Generate(jsonData);
        
        // QR kodu kaydet ve URL'ini döndür
        var url = await SaveQRCode(qrCodeImage, assignment.Id);
        return new QRCodeResult { Url = url };
    }
}
```

## 3. Zimmet Doğrulama ve İade İşlemleri

### Zimmet İade Servisi
```csharp
public class ReturnService : IReturnService
{
    private readonly IAssignmentRepository _assignmentRepo;
    private readonly ILaptopRepository _laptopRepo;
    
    public async Task<ReturnResult> ProcessReturn(string qrCode)
    {
        // QR kod verisini çöz
        var assignmentData = DecodeQRCode(qrCode);
        
        // Zimmeti bul ve doğrula
        var assignment = await _assignmentRepo.GetByIdAsync(assignmentData.AssignmentId);
        if (assignment == null || assignment.Status != AssignmentStatus.Active)
            throw new InvalidOperationException("Geçersiz zimmet kaydı");
            
        // İade işlemini gerçekleştir
        assignment.Status = AssignmentStatus.Returned;
        assignment.ActualReturnDate = DateTime.UtcNow;
        
        // Laptop durumunu güncelle
        var laptop = await _laptopRepo.GetByIdAsync(assignment.LaptopId);
        laptop.Status = LaptopStatus.Available;
        
        // Değişiklikleri kaydet
        await _assignmentRepo.UpdateAsync(assignment);
        await _laptopRepo.UpdateAsync(laptop);
        
        return new ReturnResult
        {
            Success = true,
            Message = "Zimmet başarıyla iade edildi"
        };
    }
}
```

## 4. Otomatik E-posta Hatırlatmaları

### Hatırlatma Servisi
```csharp
public class ReminderService : IReminderService
{
    private readonly IAssignmentRepository _repository;
    private readonly IEmailService _emailService;
    
    public async Task SendReturnReminders()
    {
        // Yaklaşan iadeleri bul (örn: 7 gün kala)
        var upcomingReturns = await _repository.GetUpcomingReturns(7);
        
        foreach (var assignment in upcomingReturns)
        {
            await _emailService.SendReturnReminder(new ReminderEmail
            {
                To = assignment.User.Email,
                Subject = "Zimmet İade Hatırlatması",
                DaysLeft = (assignment.ReturnDate - DateTime.UtcNow).Days,
                LaptopInfo = $"{assignment.Laptop.Model} ({assignment.Laptop.SerialNumber})",
                ReturnDate = assignment.ReturnDate
            });
        }
    }
}
```

Her servis implementasyonunda aşağıdaki önemli noktalara dikkat edilmiştir:

1. **Hata Yönetimi**: Try-catch blokları ve loglama
2. **Async/Await**: Asenkron operasyonlar için doğru kullanım
3. **Dependency Injection**: Constructor üzerinden bağımlılıkların enjekte edilmesi
4. **SOLID Prensipleri**: Tek sorumluluk ve interface segregation
5. **Clean Code**: Açıklayıcı isimlendirmeler ve kod organizasyonu
