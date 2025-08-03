# Email Notification System Implementation Plan

## 📋 Proje Hedefi
Zimmet teslim tarihi yaklaşan kullanıcılara otomatik email bildirimleri gönderen bir sistem geliştirmek.

## 🏗️ Sistem Mimarisi

### Bileşenler:
1. **Email Service** - SMTP ile email gönderimi
2. **Notification Service** - İş kuralları ve bildirim mantığı
3. **Background Job** - Otomatik görev çalıştırma
4. **SQL Server Job** - Zamanlanmış görev yürütme
5. **Template Engine** - Email şablonları

## 🎯 Gereksinimler

### Fonksiyonel Gereksinimler:
- [ ] Teslim tarihi 3 gün kala uyarı maili gönderme
- [ ] Teslim tarihi geçen laptoplar için gecikme maili
- [ ] Çoklu uyarı seviyesi (7 gün, 3 gün, 1 gün, gecikme)
- [ ] HTML email template'leri
- [ ] Email gönderim logları
- [+] SMTP ayarları yapılandırma

### Teknik Gereksinimler:
- [+] .NET 8.0 MailKit/SmtpClient kullanımı
- [+] SQL Server Agent Job oluşturma
- [+] Configuration-based SMTP settings
- [+] Error handling ve retry mechanism
- [+] Logging ve monitoring

## 📐 Detaylı İmplementasyon Planı

### Phase 1: Email Infrastructure (Adım 1-3)
#### Adım 1: Email Models ve Configuration
- [+] Email configuration model oluşturma
- [+] SMTP settings appsettings.json'a ekleme
- [+] Email template modelleri oluşturma

#### Adım 2: Email Service Implementation
- [+] IEmailService interface oluşturma
- [+] EmailService class implementasyonu
- [+] SMTP client configuration
- [+] HTML email template support

#### Adım 3: Email Templates
- [ ] Uyarı email template'i
- [ ] Gecikme email template'i
- [+] Template engine (Razor/Liquid) entegrasyonu

### Phase 2: Notification Logic (Adım 4-6)
#### Adım 4: Notification Service
- [ ] INotificationService interface
- [ ] NotificationService implementation
- [ ] İş kuralları: hangi durumlarda mail gönderileceği
- [ ] Assignment due date calculations

#### Adım 5: Database Enhancements
- [ ] Email log tablosu oluşturma
- [ ] Notification settings tablosu
- [ ] Assignment tablosuna notification fields ekleme

#### Adım 6: Background Job Service
- [ ] IBackgroundJobService interface
- [ ] Console application veya hosted service
- [ ] SQL Server job'dan çağrılabilir endpoint

### Phase 3: SQL Server Integration (Adım 7-9)
#### Adım 7: SQL Server Job Creation
- [ ] SQL Server Agent job oluşturma
- [ ] Scheduled execution (günlük, haftalık)
- [ ] Job parameters ve configuration

#### Adım 8: Error Handling & Logging
- [ ] Comprehensive error handling
- [ ] Email sending failure retry logic
- [ ] Detailed logging (Serilog integration)
- [ ] Job execution monitoring

#### Adım 9: Testing & Validation
- [ ] Unit tests for email service
- [ ] Integration tests
- [ ] Manual testing with test emails
- [ ] Performance testing

### Phase 4: Advanced Features (Adım 10-12)
#### Adım 10: Advanced Notifications
- [ ] Multiple notification levels
- [ ] User preferences (email frequency)
- [ ] Manager notifications
- [ ] Bulk assignment notifications

#### Adım 11: Monitoring & Analytics
- [ ] Email delivery tracking
- [ ] Open rate tracking (optional)
- [ ] Notification effectiveness metrics
- [ ] Dashboard for email statistics

#### Adım 12: Security & Compliance
- [ ] SMTP authentication security
- [ ] Email content sanitization
- [ ] GDPR compliance considerations
- [ ] Opt-out mechanisms

## 🗂️ Dosya Yapısı (Ekleme Planı)

```
ITAssetManagement.Web/
├── Models/
│   ├── Email/
│   │   ├── EmailConfiguration.cs
│   │   ├── EmailTemplate.cs
│   │   ├── EmailLog.cs
│   │   └── NotificationSettings.cs
│   └── Notifications/
│       ├── NotificationRequest.cs
│       └── NotificationResult.cs
├── Services/
│   ├── Interfaces/
│   │   ├── IEmailService.cs
│   │   ├── INotificationService.cs
│   │   └── IBackgroundJobService.cs
│   ├── EmailService.cs
│   ├── NotificationService.cs
│   └── BackgroundJobService.cs
├── Templates/
│   ├── Email/
│   │   ├── AssignmentDueWarning.html
│   │   ├── AssignmentOverdue.html
│   │   └── AssignmentReminder.html
├── Jobs/
│   └── NotificationJob.cs (Console App veya Hosted Service)
└── Scripts/
    └── SQL/
        ├── CreateEmailLogTable.sql
        ├── CreateNotificationSettingsTable.sql
        └── CreateSQLServerJob.sql
```

## ⚙️ Configuration Örneği

```json
{
  "EmailSettings": {
    "SmtpHost": "smtp.company.com",
    "SmtpPort": 587,
    "SmtpUsername": "noreply@company.com",
    "SmtpPassword": "password",
    "EnableSsl": true,
    "FromEmail": "noreply@company.com",
    "FromName": "IT Asset Management System"
  },
  "NotificationSettings": {
    "WarningDaysBefore": [7, 3, 1],
    "CheckFrequencyHours": 24,
    "EnableNotifications": true,
    "MaxRetryAttempts": 3
  }
}
```

## 🔄 İş Akışı

1. **SQL Server Job** günlük çalışır
2. **Background Service** tetiklenir
3. **Notification Service** assignment'ları kontrol eder
4. Kriterlere uyan kayıtlar için **Email Service** devreye girer
5. **SMTP** ile email gönderimi
6. **Email Log** kaydı oluşturulur
7. **Error handling** ve retry logic

## 📊 Bildirim Kriterleri

### Uyarı Seviyeleri:
- **7 Gün Öncesi**: İlk uyarı
- **3 Gün Öncesi**: İkinci uyarı
- **1 Gün Öncesi**: Son uyarı
- **Gecikme**: Teslim tarihi geçenler için

### Email İçeriği:
- Kullanıcı adı ve iletişim bilgileri
- Laptop detayları (marka, model, tag no)
- Teslim tarihi
- Kalan gün sayısı
- İletişim bilgileri (IT departmanı)

## 🚀 Uygulama Sırası

1. **Haftaici 1**: Phase 1 (Email Infrastructure)
2. **Haftaici 2**: Phase 2 (Notification Logic)  
3. **Haftaici 3**: Phase 3 (SQL Server Integration)
4. **Haftaici 4**: Phase 4 (Advanced Features)

## 🎯 Başlangıç Adımları (İlk 3 Adım)

### Adım 1: Email Configuration ve Models
- EmailConfiguration.cs model oluşturma
- appsettings.json'a SMTP ayarları ekleme
- EmailLog.cs model oluşturma

### Adım 2: IEmailService Interface ve Basic Implementation
- Interface tanımlama
- Basic SMTP client implementation
- DI container'a service ekleme

### Adım 3: İlk Email Template
- Simple HTML template oluşturma
- Template test endpoint oluşturma
- Manual email sending test

Bu planla başlayıp adım adım ilerleyebiliriz. Hangi adımdan başlamak istiyorsunuz?