using System.IO;
using System.Threading.Tasks;

namespace ITAssetManagement.Web.Services.Interfaces
{
    /// <summary>
    /// Barkod ve QR kod oluşturma işlemlerini yöneten servis arayüzü.
    /// Bu servis, varlık takibi için gerekli barkod/QR kod işlemlerini sağlar.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Bu servis aşağıdaki işlevleri sağlar:
    /// <list type="bullet">
    /// <item><description>Metin içeriğinden barkod oluşturma</description></item>
    /// <item><description>QR kod oluşturma ve özelleştirme</description></item>
    /// <item><description>Farklı barkod formatları desteği</description></item>
    /// <item><description>Bellek üzerinde görüntü işleme</description></item>
    /// </list>
    /// </para>
    /// </remarks>
    public interface IBarcodeService
    {
        /// <summary>
        /// Verilen metin içeriğinden barkod/QR kod oluşturur.
        /// </summary>
        /// <param name="content">Barkoda dönüştürülecek metin içeriği</param>
        /// <returns>Oluşturulan barkod görüntüsünü içeren MemoryStream</returns>
        /// <remarks>
        /// <para>
        /// Bu metot şu özelliklere sahiptir:
        /// <list type="bullet">
        /// <item><description>PNG formatında görüntü üretir</description></item>
        /// <item><description>Yüksek çözünürlüklü çıktı sağlar</description></item>
        /// <item><description>UTF-8 karakter desteği sunar</description></item>
        /// </list>
        /// </para>
        /// </remarks>
        MemoryStream GenerateBarcode(string content);
    }
}
