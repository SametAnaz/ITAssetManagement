# Görünümler Genel Bakış

IT Varlık Yönetimi uygulaması, kullanıcı arayüzünü oluşturmak için ASP.NET Core MVC görünümlerini kullanır. Bu bölüm, görünüm yapısını, kurallarını ve özelliklerini belgelemektedir.

## Görünüm Yapısı

```
Views/
├── Shared/                     # Paylaşılan layout ve partial'lar
│   ├── _Layout.cshtml         # Ana layout şablonu
│   ├── _ValidationScripts.cshtml
│   └── Error.cshtml
├── Assignments/               # Zimmet yönetimi görünümleri
│   ├── Index.cshtml
│   ├── Create.cshtml
│   ├── Edit.cshtml
│   ├── Details.cshtml
│   └── Delete.cshtml
├── Laptops/                   # Dizüstü bilgisayar yönetimi görünümleri
│   ├── Index.cshtml
│   ├── Create.cshtml
│   ├── Edit.cshtml
│   ├── Details.cshtml
│   ├── Delete.cshtml
│   └── DeletedLaptops.cshtml
├── Users/                     # Kullanıcı yönetimi görünümleri
│   ├── Index.cshtml
│   ├── Create.cshtml
│   ├── Edit.cshtml
│   ├── Details.cshtml
│   └── Delete.cshtml
├── Auth/                      # Kimlik doğrulama görünümleri
│   └── Login.cshtml
├── EmailTest/                 # E-posta test görünümleri
│   └── Index.cshtml
├── Home/                      # Ana sayfa ve panel görünümleri
│   └── Index.cshtml
├── _ViewImports.cshtml        # Ortak importlar ve tag helper'lar
└── _ViewStart.cshtml          # Ortak görünüm yapılandırması
```

## Görünüm Özellikleri

- Bootstrap 5 ile duyarlı tasarım
- İstemci tarafı işlemler için jQuery
- Yerleşik form doğrulama
- Dinamik güncellemeler için AJAX desteği
- QR kod entegrasyonu
- Dosya yükleme işleme
- Toast bildirimleri
- Modal diyaloglar

## Görünüm En İyi Uygulamaları

- Tüm görünümler _Layout.cshtml'den miras alır
- Tutarlı isimlendirme kuralları
- Yeniden kullanılabilir bileşenler için partial view'lar
- İstemci tarafı doğrulama etkin
- Önce mobil duyarlı tasarım
- Temiz sorumluluk ayrımı
- Uygun model bağlama

[Learn more about specific view categories](categories.md)
