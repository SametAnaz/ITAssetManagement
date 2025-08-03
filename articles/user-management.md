# Kullanıcı Yönetimi

Bu dokümantasyon IT Varlık Yönetim Sistemi'ndeki kullanıcı yönetimi işlemlerini detaylı olarak açıklamaktadır.

## İçindekiler

1. [Kullanıcı Rolleri](#kullanıcı-rolleri)
2. [Kullanıcı İşlemleri](#kullanıcı-işlemleri)
3. [Yetkilendirme](#yetkilendirme)
4. [Güvenlik Politikaları](#güvenlik-politikaları)

## Kullanıcı Rolleri

### Administrator
- Sistem genelinde tam yetki
- Kullanıcı yönetimi
- Rol atama/düzenleme
- Sistem ayarlarını değiştirme

### Asset Manager
- Varlık ekleme/düzenleme/silme
- Zimmet atama/iade alma
- Raporları görüntüleme
- Kullanıcı listesini görüntüleme

### Department Manager
- Departman bazlı zimmet onaylama
- Departman raporlarını görüntüleme
- Departman kullanıcılarını yönetme

### Standard User
- Zimmet listesini görüntüleme
- Zimmet taleplerini oluşturma
- Kişisel raporları görüntüleme

## Kullanıcı İşlemleri

### Yeni Kullanıcı Ekleme
1. "Kullanıcılar" menüsünden "Yeni Kullanıcı" butonuna tıklayın
2. Gerekli bilgileri doldurun:
   - Ad Soyad
   - E-posta (kurum e-postası)
   - Departman
   - Rol
3. "Kaydet" butonuna tıklayın

```csharp
// Örnek Kullanıcı Oluşturma Kodu
var user = new User
{
    FirstName = "Ahmet",
    LastName = "Yılmaz",
    Email = "ahmet.yilmaz@sirket.com",
    Department = "IT",
    Role = UserRoles.AssetManager
};
```

### Kullanıcı Düzenleme
1. Kullanıcı listesinden ilgili kullanıcıyı bulun
2. "Düzenle" butonuna tıklayın
3. Bilgileri güncelleyin
4. "Kaydet" butonuna tıklayın

### Kullanıcı Deaktive Etme
- Kullanıcılar silinmez, deaktive edilir
- Deaktive edilen kullanıcının zimmetleri otomatik iade sürecine girer
- Geçmiş kayıtlar korunur

## Yetkilendirme

### Role-Based Access Control (RBAC)
```csharp
[Authorize(Roles = "Administrator,AssetManager")]
public class AssetsController : Controller
{
    [Authorize(Roles = "Administrator")]
    public IActionResult Delete(int id)
    {
        // Silme işlemi
    }
}
```

### Özel Yetkilendirmeler
- Departman bazlı erişim kısıtlamaları
- İşlem bazlı yetkiler
- IP bazlı erişim kontrolleri

## Güvenlik Politikaları

### Şifre Politikası
- Minimum 8 karakter
- En az 1 büyük harf
- En az 1 küçük harf
- En az 1 rakam
- En az 1 özel karakter
- 90 günde bir şifre değişimi

### Oturum Güvenliği
- 30 dakika inaktivite sonrası otomatik çıkış
- Tek oturum politikası
- Başarısız giriş denemesi limiti (5 deneme)

### İki Faktörlü Doğrulama (2FA)
1. E-posta ile doğrulama
2. SMS ile doğrulama (opsiyonel)
3. Authenticator app desteği

## Raporlama

### Kullanıcı Aktivite Raporları
- Giriş kayıtları
- İşlem geçmişi
- Zimmet hareketleri

### Yönetici Raporları
- Kullanıcı listesi
- Rol dağılımları
- Departman bazlı istatistikler

## Sorun Giderme

### Sık Karşılaşılan Sorunlar
1. **Şifremi Unuttum**
   - Self-servis şifre sıfırlama
   - Yönetici üzerinden sıfırlama

2. **Hesap Kilitleri**
   - Otomatik kilit açma süresi
   - Yönetici müdahalesi

3. **Yetki Sorunları**
   - Rol kontrolleri
   - Cache temizleme
   - Oturum yenileme

## En İyi Uygulamalar

1. **Kullanıcı Oluşturma**
   - Kurumsal e-posta zorunluluğu
   - Departman bilgisi doğruluğu
   - Uygun rol ataması

2. **Güvenlik**
   - Düzenli yetki kontrolleri
   - Periyodik şifre değişimleri
   - Aktivite loglarının takibi

3. **Bakım**
   - Düzenli kullanıcı listesi temizliği
   - Deaktive edilmiş hesapların kontrolü
   - Yetki dağılımlarının gözden geçirilmesi
