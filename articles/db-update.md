# Veritabanı Güncellemesi: MySQL'den MS SQL Server'a Geçiş

Bu dokümantasyon, IT Varlık Yönetim Sistemi'nin veritabanını MySQL'den MS SQL Server'a geçiş sürecini detaylı olarak anlatmaktadır.

## İçindekiler

1. [Mevcut Veritabanı Yapısı](#mevcut-veritabanı-yapısı)
2. [Geçiş Öncesi Hazırlık](#geçiş-öncesi-hazırlık)
3. [MS SQL Server Kurulumu](#ms-sql-server-kurulumu)
4. [Veritabanı Geçişi](#veritabanı-geçişi)
5. [Entity Framework Konfigürasyonu](#entity-framework-konfigürasyonu)
6. [Veri Doğrulama](#veri-doğrulama)

## Mevcut Veritabanı Yapısı

### Temel Tablolar ve İlişkileri

```sql
-- Kullanıcılar Tablosu
CREATE TABLE Users (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    Username NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) UNIQUE NOT NULL,
    Department NVARCHAR(50),
    CreatedDate DATETIME DEFAULT CURRENT_TIMESTAMP,
    IsActive BIT DEFAULT 1
);

-- Laptoplar Tablosu
CREATE TABLE Laptops (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    SerialNumber NVARCHAR(50) UNIQUE NOT NULL,
    Model NVARCHAR(100) NOT NULL,
    Brand NVARCHAR(50) NOT NULL,
    PurchaseDate DATETIME,
    Status INT NOT NULL DEFAULT 1, -- 1: Available, 2: Assigned, 3: In Service
    LastMaintenanceDate DATETIME,
    NextMaintenanceDate DATETIME,
    Notes TEXT
);

-- Laptop Fotoğrafları Tablosu
CREATE TABLE LaptopPhotos (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    LaptopId INT NOT NULL,
    PhotoUrl NVARCHAR(255) NOT NULL,
    UploadDate DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (LaptopId) REFERENCES Laptops(Id)
);

-- Zimmetler Tablosu
CREATE TABLE Assignments (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    UserId INT NOT NULL,
    LaptopId INT NOT NULL,
    AssignmentDate DATETIME NOT NULL,
    ReturnDate DATETIME,
    Status INT NOT NULL DEFAULT 1, -- 1: Active, 2: Returned, 3: Overdue
    Notes TEXT,
    QRCodeUrl NVARCHAR(255),
    FOREIGN KEY (UserId) REFERENCES Users(Id),
    FOREIGN KEY (LaptopId) REFERENCES Laptops(Id)
);

-- Laptop İşlem Logları
CREATE TABLE LaptopLogs (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    LaptopId INT NOT NULL,
    Operation NVARCHAR(50) NOT NULL,
    Description TEXT,
    OperationDate DATETIME DEFAULT CURRENT_TIMESTAMP,
    OperatedBy INT,
    FOREIGN KEY (LaptopId) REFERENCES Laptops(Id),
    FOREIGN KEY (OperatedBy) REFERENCES Users(Id)
);

-- E-posta Logları
CREATE TABLE EmailLogs (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    ToAddress NVARCHAR(100) NOT NULL,
    Subject NVARCHAR(200) NOT NULL,
    Body TEXT,
    SentDate DATETIME DEFAULT CURRENT_TIMESTAMP,
    Status INT NOT NULL DEFAULT 0, -- 0: Pending, 1: Sent, 2: Failed
    ErrorMessage TEXT
);
```

## Geçiş Öncesi Hazırlık

1. **Veritabanı Yedekleme**
   ```bash
   mysqldump -u root -p ITAssetManagement > backup.sql
   ```

2. **Veri Doğrulama**
   ```sql
   -- Veri bütünlüğü kontrolü
   SELECT COUNT(*) FROM Users;
   SELECT COUNT(*) FROM Laptops;
   SELECT COUNT(*) FROM Assignments;
   
   -- İlişki kontrolü
   SELECT * FROM Assignments a 
   LEFT JOIN Users u ON a.UserId = u.Id 
   WHERE u.Id IS NULL;
   ```

## MS SQL Server Kurulumu

1. **SQL Server Kurulumu**
   - SQL Server 2019 Express Edition indirin
   - Basic kurulum seçeneğini kullanın
   - Windows Authentication modunu seçin

2. **Veritabanı Oluşturma**
   ```sql
   CREATE DATABASE ITAssetManagement;
   GO
   
   USE ITAssetManagement;
   GO
   ```

## Veritabanı Geçişi

### MS SQL Server Tablo Yapısı

```sql
-- Kullanıcılar Tablosu
CREATE TABLE Users (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Username NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) UNIQUE NOT NULL,
    Department NVARCHAR(50),
    CreatedDate DATETIME2 DEFAULT GETDATE(),
    IsActive BIT DEFAULT 1
);

-- Laptoplar Tablosu
CREATE TABLE Laptops (
    Id INT PRIMARY KEY IDENTITY(1,1),
    SerialNumber NVARCHAR(50) UNIQUE NOT NULL,
    Model NVARCHAR(100) NOT NULL,
    Brand NVARCHAR(50) NOT NULL,
    PurchaseDate DATETIME2,
    Status INT NOT NULL DEFAULT 1,
    LastMaintenanceDate DATETIME2,
    NextMaintenanceDate DATETIME2,
    Notes NVARCHAR(MAX)
);

-- Diğer tablolar için benzer dönüşümler...
```

### Veri Tipi Dönüşümleri

| MySQL | MS SQL Server | Not |
|-------|--------------|-----|
| AUTO_INCREMENT | IDENTITY(1,1) | Primary key için otomatik artış |
| TEXT | NVARCHAR(MAX) | Uzun metinler için |
| DATETIME | DATETIME2 | Daha yüksek hassasiyet |
| BIT | BIT | Aynı kullanım |
| FOREIGN KEY | FOREIGN KEY | Syntax farklı |

## Entity Framework Konfigürasyonu

### 1. Connection String Güncelleme
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=ITAssetManagement;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
  }
}
```

### 2. DbContext Güncelleme
```csharp
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(
                "Server=localhost\\SQLEXPRESS;Database=ITAssetManagement;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True",
                x => x.MigrationsHistoryTable("__EFMigrationsHistory")
            );
        }
    }

    // Mevcut DbSet tanımlamaları aynı kalacak
    public DbSet<User> Users { get; set; }
    public DbSet<Laptop> Laptops { get; set; }
    public DbSet<Assignment> Assignments { get; set; }
    // ...
}
```

### 3. Migration Oluşturma
```powershell
# Eski migrasyonları temizle
Remove-Migration

# Yeni migration oluştur
Add-Migration InitialCreate

# Veritabanını güncelle
Update-Database
```

## Veri Doğrulama

### 1. Veri Sayımı Kontrolü
```sql
-- MySQL'deki kayıt sayıları
SELECT 'Users' as Table_Name, COUNT(*) as Count FROM Users
UNION ALL
SELECT 'Laptops', COUNT(*) FROM Laptops
UNION ALL
SELECT 'Assignments', COUNT(*) FROM Assignments;

-- MS SQL'deki kayıt sayıları ile karşılaştır
SELECT 'Users' as Table_Name, COUNT(*) as Count FROM Users
UNION ALL
SELECT 'Laptops', COUNT(*) FROM Laptops
UNION ALL
SELECT 'Assignments', COUNT(*) FROM Assignments;
```

### 2. İlişki Kontrolü
```sql
-- Geçersiz zimmet kaydı kontrolü
SELECT a.* 
FROM Assignments a
LEFT JOIN Users u ON a.UserId = u.Id
LEFT JOIN Laptops l ON a.LaptopId = l.Id
WHERE u.Id IS NULL OR l.Id IS NULL;
```

### 3. Veri Bütünlüğü Testi
```sql
-- Benzersiz alan kontrolü
SELECT Email, COUNT(*) 
FROM Users 
GROUP BY Email 
HAVING COUNT(*) > 1;

SELECT SerialNumber, COUNT(*) 
FROM Laptops 
GROUP BY SerialNumber 
HAVING COUNT(*) > 1;
```

## Sorun Giderme

### Sık Karşılaşılan Hatalar

1. **Bağlantı Hataları**
   ```
   "A network-related or instance-specific error occurred..."
   ```
   Çözüm:
   - SQL Server servisinin çalıştığını kontrol edin
   - Windows Authentication yetkilerini kontrol edin
   - Firewall ayarlarını kontrol edin

2. **Migration Hataları**
   ```
   "There is already an object named 'xxx' in the database."
   ```
   Çözüm:
   ```sql
   -- Tüm tabloları temizle
   EXEC sp_MSforeachtable @command1="DROP TABLE ?"
   ```

3. **Identity Insert Hataları**
   ```
   "Cannot insert explicit value for identity column..."
   ```
   Çözüm:
   ```sql
   SET IDENTITY_INSERT [TableName] ON;
   -- Insert işlemleri
   SET IDENTITY_INSERT [TableName] OFF;
   ```

## Rollback Planı

1. **MySQL Yedeğini Geri Yükleme**
   ```bash
   mysql -u root -p ITAssetManagement < backup.sql
   ```

2. **Connection String Değiştirme**
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=localhost;Database=ITAssetManagement;User=root;Password=yourpassword;"
     }
   }
   ```

3. **Uygulama Havuzunu Yeniden Başlatma**
   ```powershell
   Stop-Website "ITAssetManagement"
   Start-Website "ITAssetManagement"
   ```
