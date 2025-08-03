# Varlık Yönetimi

Bu dokümantasyon IT Varlık Yönetim Sistemi'ndeki varlık (asset) yönetimi işlemlerini detaylı olarak açıklamaktadır.

## İçindekiler

1. [Varlık Kategorileri](#varlık-kategorileri)
2. [Varlık İşlemleri](#varlık-işlemleri)
3. [Envanter Yönetimi](#envanter-yönetimi)
4. [QR Kod Sistemi](#qr-kod-sistemi)

## Varlık Kategorileri

### Laptoplar
- Marka/Model bilgisi
- Seri numarası
- Donanım özellikleri
- Garanti durumu

### Masaüstü Bilgisayarlar
- Sistem bileşenleri
- Lokasyon bilgisi
- Bağlı ekipmanlar

### Ekipmanlar
- Monitörler
- Yazıcılar
- Ağ cihazları
- Aksesuar ve çevre birimleri

## Varlık İşlemleri

### Yeni Varlık Ekleme
```csharp
var laptop = new Laptop
{
    Brand = "Dell",
    Model = "Latitude 5520",
    SerialNumber = "DL5520X123",
    Specifications = new LaptopSpecifications
    {
        Processor = "Intel i7 11th Gen",
        RAM = "16GB",
        Storage = "512GB SSD",
        GPU = "Intel Iris Xe"
    },
    PurchaseDate = DateTime.Now,
    WarrantyEndDate = DateTime.Now.AddYears(3)
};
```

### Varlık Durumları
1. **Müsait**
   - Zimmete verilebilir
   - Stokta mevcut

2. **Zimmette**
   - Aktif kullanımda
   - Zimmet sahibi bilgisi

3. **Serviste**
   - Arıza/bakım durumu
   - Beklenen dönüş tarihi

4. **Emekli**
   - Kullanım dışı
   - Hurda/satış durumu

## Envanter Yönetimi

### Stok Takibi
- Anlık envanter durumu
- Minimum stok uyarıları
- Departman bazlı dağılım

### Bakım Planlaması
1. **Periyodik Bakımlar**
   - Bakım takvimi
   - Otomatik hatırlatmalar
   - Servis geçmişi

2. **Garanti Takibi**
   - Garanti bitiş tarihleri
   - Garanti kapsamı
   - Servis anlaşmaları

## QR Kod Sistemi

### QR Kod Oluşturma
```csharp
public async Task<string> GenerateQRCode(Asset asset)
{
    var data = new AssetQRData
    {
        AssetId = asset.Id,
        SerialNumber = asset.SerialNumber,
        Type = asset.Type,
        LastUpdated = DateTime.UtcNow
    };

    return await _qrService.GenerateCode(data);
}
```

### QR Kod İçeriği
- Varlık ID
- Seri numarası
- Kategori bilgisi
- Son güncelleme tarihi

### QR Kod Kullanımı
1. Zimmet işlemleri
2. Hızlı envanter sayımı
3. Servis takibi

## Raporlama

### Envanter Raporları
- Toplam varlık listesi
- Kategori bazlı dağılımlar
- Durum bazlı raporlar

### Maliyet Analizleri
- Satın alma maliyetleri
- Bakım/onarım giderleri
- Toplam sahip olma maliyeti (TCO)

### Kullanım İstatistikleri
- Zimmet süreleri
- Arıza sıklıkları
- Departman bazlı kullanım

## Varlık Yaşam Döngüsü

### 1. Tedarik
- Satın alma süreci
- Teslim alma
- İlk kayıt

### 2. Aktif Kullanım
- Zimmet işlemleri
- Bakım/onarım
- Yer değişiklikleri

### 3. Emeklilik
- Kullanım dışı bırakma
- Hurda/satış işlemleri
- Kayıt silme

## En İyi Uygulamalar

1. **Varlık Kaydı**
   - Detaylı bilgi girişi
   - Doğru kategorizasyon
   - Belge ve fotoğraf ekleme

2. **Envanter Yönetimi**
   - Düzenli sayım
   - Lokasyon takibi
   - Durumu güncelleme

3. **Bakım Planlaması**
   - Proaktif bakım takvimi
   - Servis geçmişi tutma
   - Garanti takibi

## Sorun Giderme

### Sık Karşılaşılan Sorunlar
1. **QR Kod Okuma**
   - Kod kalitesi kontrolü
   - Kamera ayarları
   - Aydınlatma koşulları

2. **Envanter Tutarsızlığı**
   - Çift kayıt kontrolü
   - Zimmet geçmişi kontrolü
   - Manuel sayım
