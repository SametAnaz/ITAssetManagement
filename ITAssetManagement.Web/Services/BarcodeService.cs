using System;
using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using ZXing;
using ZXing.Common;
using ZXing.ImageSharp;
using ITAssetManagement.Web.Services.Interfaces;

namespace ITAssetManagement.Web.Services
{
    /// <summary>
    /// Barkod ve QR kod oluşturma servisi.
    /// ZXing kütüphanesi kullanarak çeşitli formatlarda barkod/QR kod üretir.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Bu servis şu özellikleri sağlar:
    /// <list type="bullet">
    /// <item><description>CODE_128 formatında barkod üretimi</description></item>
    /// <item><description>Özelleştirilebilir boyut ve kenar boşlukları</description></item>
    /// <item><description>Yüksek kaliteli PNG çıktısı</description></item>
    /// <item><description>Bellek optimizasyonlu işlem</description></item>
    /// </list>
    /// </para>
    /// </remarks>
    public class BarcodeService : IBarcodeService
    {
        /// <summary>
        /// Verilen metin içeriğinden CODE_128 formatında barkod oluşturur.
        /// </summary>
        /// <param name="content">Barkoda dönüştürülecek metin içeriği</param>
        /// <returns>PNG formatında barkod görüntüsünü içeren MemoryStream</returns>
        /// <remarks>
        /// <para>
        /// Oluşturulan barkodun özellikleri:
        /// <list type="bullet">
        /// <item><description>300x150 piksel boyutunda</description></item>
        /// <item><description>10 piksel kenar boşluğu</description></item>
        /// <item><description>RGBA32 renk formatı</description></item>
        /// <item><description>PNG sıkıştırma ile optimize edilmiş</description></item>
        /// </list>
        /// </para>
        /// </remarks>
        public MemoryStream GenerateBarcode(string content)
        {
            var barcodeWriter = new ZXing.ImageSharp.BarcodeWriter<Rgba32>
            {
                Format = BarcodeFormat.CODE_128,
                Options = new EncodingOptions
                {
                    Width = 300,
                    Height = 150,
                    Margin = 10,
                    PureBarcode = false
                }
            };

            using var image = barcodeWriter.Write(content);
            var memoryStream = new MemoryStream();
            image.Save(memoryStream, new PngEncoder());
            memoryStream.Position = 0;
            return memoryStream;
        }
    }
}
