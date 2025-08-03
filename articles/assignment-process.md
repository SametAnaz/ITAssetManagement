# Zimmet İşlemleri

Bu dokümantasyon IT Varlık Yönetim Sistemi'ndeki zimmet süreçlerini detaylı olarak açıklamaktadır.

## İçindekiler

1. [Zimmet Süreci](#zimmet-süreci)
2. [İade Süreci](#i̇ade-süreci)
3. [Otomatik Bildirimler](#otomatik-bildirimler)
4. [Raporlama](#raporlama)

## Zimmet Süreci

### Zimmet Oluşturma
1. **Manuel Zimmet**
   ```csharp
   var assignment = new Assignment
   {
       UserId = userId,
       AssetId = assetId,
       StartDate = DateTime.Now,
       EndDate = DateTime.Now.AddMonths(6),
       Notes = "Proje süresince kullanım"
   };
   ```

2. **QR Kod ile Hızlı Zimmet**
   - Varlık QR kodunu tarama
   - Kullanıcı seçimi
   - Süre belirleme
   - Otomatik kayıt

### Zimmet Onay Süreci

1. **Talep Aşaması**
   - Kullanıcı talebi
   - Yönetici onayı
   - IT onayı

2. **Teslim Aşaması**
   - Fiziksel kontrol
   - Teslim formu
   - QR kod etiketleme

## İade Süreci

### İade İşlemi
```csharp
public async Task<ReturnResult> ProcessReturn(int assignmentId)
{
    var assignment = await _assignmentRepo.GetByIdAsync(assignmentId);
    
    // Fiziksel kontrol
    var inspection = await InspectAsset(assignment.AssetId);
    
    if (inspection.HasDamage)
    {
        await CreateDamageReport(inspection);
    }
    
    assignment.Status = AssignmentStatus.Returned;
    assignment.ReturnDate = DateTime.Now;
    
    await _assignmentRepo.UpdateAsync(assignment);
    await _emailService.SendReturnConfirmation(assignment);
    
    return new ReturnResult { Success = true };
}
```

### İade Kontrol Listesi
- [ ] Fiziksel hasar kontrolü
- [ ] Aksesuar kontrolü
- [ ] Yazılım kontrolü
- [ ] Temizlik kontrolü

## Otomatik Bildirimler

### Zimmet Bildirimleri
1. **Yeni Zimmet**
   - Kullanıcıya bildirim
   - Yöneticiye bildirim
   - QR kod paylaşımı

2. **Süre Hatırlatmaları**
   - 1 ay kala
   - 1 hafta kala
   - Son gün

### İade Bildirimleri
- İade onayı
- Hasar raporu
- Geç iade uyarısı

## Zimmet Kuralları

### Süre Politikaları
- Minimum süre: 1 ay
- Maksimum süre: 24 ay
- Uzatma koşulları

### Sorumluluk Kuralları
1. **Kullanıcı Sorumlulukları**
   - Özenli kullanım
   - Zamanında iade
   - Hasar bildirimi

2. **Departman Sorumlulukları**
   - Bütçe yönetimi
   - Kullanım onayı
   - Takip sorumluluğu

## Raporlama

### Zimmet Raporları
- Aktif zimmetler
- Geçmiş zimmetler
- Departman bazlı dağılım

### Analiz Raporları
```sql
SELECT 
    d.Name AS Department,
    COUNT(a.Id) AS ActiveAssignments,
    AVG(DATEDIFF(day, a.StartDate, a.EndDate)) AS AvgDuration
FROM Assignments a
JOIN Users u ON a.UserId = u.Id
JOIN Departments d ON u.DepartmentId = d.Id
WHERE a.Status = 'Active'
GROUP BY d.Name
```

## Özel Durumlar

### Acil Zimmet
- Hızlı onay süreci
- Minimum dokümantasyon
- Sonradan tamamlama

### Toplu Zimmet
- Excel ile import
- Batch processing
- Toplu bildirim

### Geçici Zimmet
- Kısa süreli kullanım
- Basitleştirilmiş süreç
- Hızlı iade

## En İyi Uygulamalar

1. **Zimmet Oluşturma**
   - Detaylı dokümanlar
   - Net süre belirleme
   - Kullanım amacı

2. **Takip ve Kontrol**
   - Düzenli envanter kontrolü
   - Süre takibi
   - İade planlaması

3. **İade Süreci**
   - Detaylı kontrol
   - Belgelendirme
   - Hızlı işlem

## Sorun Giderme

### Sık Karşılaşılan Sorunlar

1. **Geç İade**
   - Otomatik uyarılar
   - Eskalasyon süreci
   - Yaptırım politikası

2. **Hasar Durumu**
   - Hasar tespit
   - Sorumluluk belirleme
   - Onarım süreci

3. **Kayıp/Çalıntı**
   - Bildirim süreci
   - Sigorta işlemleri
   - Yasal süreç
