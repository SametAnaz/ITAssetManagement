# Kurulum Kılavuzu

Bu kılavuz, IT Varlık Yönetim Sistemi'nin kurulum ve konfigürasyon adımlarını detaylı olarak anlatmaktadır.

## İçindekiler

1. [Sistem Gereksinimleri](#sistem-gereksinimleri)
2. [Geliştirme Ortamı Kurulumu](#geliştirme-ortamı-kurulumu)
3. [Veritabanı Kurulumu](#veritabanı-kurulumu)
4. [Proje Kurulumu](#proje-kurulumu)
5. [Konfigürasyon](#konfigürasyon)
6. [İlk Çalıştırma](#ilk-çalıştırma)

## Sistem Gereksinimleri

### Minimum Gereksinimler

- **İşletim Sistemi**: Windows 10/11, Linux veya macOS
- **RAM**: En az 4GB (8GB önerilen)
- **İşlemci**: Dual Core 2.0 GHz veya üzeri
- **Disk Alanı**: En az 10GB boş alan

### Yazılım Gereksinimleri

- **.NET SDK**: 7.0 veya üzeri
- **SQL Server**: 2019 veya üzeri (Express Edition yeterli)
- **Visual Studio**: 2022 veya üzeri (Community Edition yeterli)
- **Git**: En son versiyon

## Geliştirme Ortamı Kurulumu

1. **.NET SDK Kurulumu**
   ```powershell
   winget install Microsoft.DotNet.SDK.7
   ```

2. **Visual Studio Kurulumu**
   - [Visual Studio İndirme Sayfası](https://visualstudio.microsoft.com/downloads/)
   - Kurulum sırasında şu workload'ları seçin:
     - ASP.NET and web development
     - .NET desktop development
     - Data storage and processing

3. **SQL Server Kurulumu**
   - [SQL Server Express İndirme](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
   - Kurulum sırasında "Basic" kurulum seçeneğini tercih edebilirsiniz
   - SQL Server Management Studio'yu da kurmanız önerilir

## Veritabanı Kurulumu

1. **Veritabanı Oluşturma**
   ```sql
   CREATE DATABASE ITAssetManagement;
   GO
   ```

2. **Kullanıcı Oluşturma ve Yetkilendirme**
   ```sql
   USE ITAssetManagement;
   CREATE LOGIN ITAssetUser WITH PASSWORD = 'güçlü-bir-şifre';
   CREATE USER ITAssetUser FOR LOGIN ITAssetUser;
   ALTER ROLE db_owner ADD MEMBER ITAssetUser;
   GO
   ```

## Proje Kurulumu

1. **Projeyi Klonlama**
   ```powershell
   git clone https://github.com/kullanici/ITAssetManagement.git
   cd ITAssetManagement
   ```

2. **Bağımlılıkları Yükleme**
   ```powershell
   dotnet restore
   ```

3. **Veritabanı Migration**
   ```powershell
   cd ITAssetManagement.Web
   dotnet ef database update
   ```

## Konfigürasyon

1. **appsettings.json Düzenleme**
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=ITAssetManagement;Trusted_Connection=True;MultipleActiveResultSets=true"
     },
     "EmailConfiguration": {
       "SmtpServer": "smtp.sirketiniz.com",
       "SmtpPort": 587,
       "SmtpUsername": "bildirim@sirketiniz.com",
       "SmtpPassword": "email-sifresi"
     }
   }
   ```

2. **Uygulama Ayarları**
   - `appsettings.Development.json` dosyasını kendi geliştirme ortamınıza göre düzenleyin
   - Gerekirse ortama özel appsettings dosyaları oluşturun (örn: `appsettings.Production.json`)

## İlk Çalıştırma

1. **Projeyi Build Etme**
   ```powershell
   dotnet build
   ```

2. **Uygulamayı Çalıştırma**
   ```powershell
   dotnet run
   ```

3. **İlk Kullanıcı Oluşturma**
   - Tarayıcıda `https://localhost:5001` adresine gidin
   - "Register" sayfasından ilk admin kullanıcıyı oluşturun
   - Admin kullanıcı bilgileri:
     - Email: admin@sirketiniz.com
     - Şifre: güçlü-bir-şifre
     - Rol: Administrator

## Sorun Giderme

### Sık Karşılaşılan Hatalar

1. **Veritabanı Bağlantı Hatası**
   - Connection string'i kontrol edin
   - SQL Server servisinin çalıştığından emin olun
   - Windows Authentication yetkilerinizi kontrol edin

2. **Migration Hataları**
   ```powershell
   # Migration'ları sıfırlama
   dotnet ef database drop
   dotnet ef database update
   ```

3. **Port Çakışması**
   - `Properties/launchSettings.json` dosyasından port ayarlarını değiştirin
   - Kullanılan portların başka uygulamalar tarafından kullanılmadığından emin olun

### Güvenlik Kontrol Listesi

- [ ] Varsayılan şifrelerin değiştirilmesi
- [ ] Güvenlik duvarı kurallarının kontrolü
- [ ] SSL sertifikasının kurulumu
- [ ] Kullanıcı yetkilerinin kontrolü
- [ ] Veritabanı yedekleme planının oluşturulması

## Sonraki Adımlar

Kurulum tamamlandıktan sonra şu dokümanlara göz atmanızı öneririz:

1. [Kullanıcı Yönetimi](user-management.md)
2. [Varlık Ekleme](asset-management.md)
3. [Zimmet İşlemleri](assignment-process.md)

## Destek

Kurulum sırasında sorun yaşarsanız:

- 📧 Email: support@itasset.com
- 💬 GitHub Issues
- 📱 Teknik Destek: +90 (xxx) xxx xx xx
