# IT Varlık Yönetimi Sistem Mimarisi

## Sistem Genel Bakış

IT Varlık Yönetimi, temiz mimari prensipleri ve SOLID tasarım desenlerini takip eden ASP.NET Core MVC kullanılarak inşa edilmiştir. Sistem, IT varlıklarını (özellikle dizüstü bilgisayarları) edinimden emekliliğe kadar tüm yaşam döngüsü boyunca yönetmek için tasarlanmıştır.

### Temel Tasarım Prensipleri

1. **Temiz Mimari (Clean Architecture)**
   - Net sorumluluk ayrımı
   - İçe doğru bağımlılıklar
   - İş kurallarının UI ve veritabanından izolasyonu
   - Kolay test edilebilirlik ve bakım

2. **SOLID Prensipleri**
   - **Tek Sorumluluk**: Her sınıfın değişmek için tek bir nedeni olmalı
   - **Açık/Kapalı**: Genişletmeye açık, değişime kapalı
   - **Liskov Yerine Geçme**: Türetilmiş sınıflar temel sınıfların yerine geçebilmeli
   - **Arayüz Ayrımı**: Özel arayüzler genel olanlara tercih edilmeli
   - **Bağımlılığın Ters Çevrilmesi**: Üst seviye modüller alt seviye modüllere bağımlı olmamalı

3. **Domain Odaklı Tasarım (DDD)**
   - Zengin domain modelleri
   - Domain katmanında iş mantığı
   - Aggregate kökleri (Varlık, Kullanıcı)
   - Değer nesneleri (E-posta, SeriNumarası)

### Teknoloji Yığını

1. **Backend**
   - ASP.NET Core 7.0
   - C# 11.0
   - Entity Framework Core 7.0
   - SQL Server 2022

2. **Frontend**
   - ASP.NET Core MVC Views
   - Bootstrap 5.2
   - jQuery 3.6
   - Modern JavaScript (ES6+)

3. **Geliştirme Araçları**
   - Visual Studio 2022
   - Azure DevOps
   - Git versiyon kontrolü
   - DocFX dokümantasyon

### Altyapı

1. **Barındırma**
   - Azure App Service
   - Azure SQL Veritabanı
   - Azure Key Vault (gizli bilgiler için)
   - Azure Storage (dosyalar için)

2. **İzleme**
   - Application Insights
   - Log Analytics
   - Özel telemetri
   - Sağlık kontrolleri

3. **Güvenlik**
   - Azure AD entegrasyonu
   - SSL/TLS şifreleme
   - SQL Always Encrypted
   - Anti-forgery koruması

## Architecture Layers

```
ITAssetManagement/
├── Presentation Layer (MVC)
│   ├── Controllers/
│   ├── Views/
│   └── wwwroot/
├── Business Layer
│   ├── Services/
│   └── Interfaces/
├── Data Access Layer
│   ├── Repositories/
│   ├── DbContext/
│   └── Migrations/
└── Cross-Cutting Concerns
    ├── Extensions/
    ├── Helpers/
    └── Models/
```

### 1. Presentation Layer (MVC)
- **Controllers**: Handle HTTP requests and user interactions
- **Views**: Render the UI using Razor syntax
- **wwwroot**: Static files (CSS, JS, images)

### 2. Business Layer
- **Services**: Implement business logic
- **Interfaces**: Define service contracts
- **Validation**: Business rule validation

### 3. Data Access Layer
- **Repositories**: Data access patterns
- **DbContext**: Entity Framework configuration
- **Migrations**: Database schema changes

### 4. Cross-Cutting Concerns
- **Extensions**: Utility extensions
- **Helpers**: Common functionality
- **Models**: Domain and view models

## Key Components

### Authentication & Authorization
- ASP.NET Core Identity
- Role-based access control
- JWT tokens for API access

### Database
- SQL Server
- Entity Framework Core
- Code-first migrations
- Repository pattern

### Email System
- SMTP integration
- Email templates
- Queue-based processing
- Retry mechanism

### QR Code System
- QR generation
- Scanner integration
- Asset linking

[Learn more about system components](components.md)
