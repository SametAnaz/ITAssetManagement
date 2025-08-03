namespace ITAssetManagement.Web.Models
{
    /// <summary>
    /// Uygulama genelinde hata sayfalarında kullanılan görünüm modeli.
    /// Bu model, hata detaylarını ve izleme bilgilerini kullanıcıya göstermek için kullanılır.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Bu model aşağıdaki senaryolarda kullanılır:
    /// <list type="bullet">
    /// <item><description>404 Not Found hataları</description></item>
    /// <item><description>500 Internal Server Error durumları</description></item>
    /// <item><description>Özel hata sayfaları</description></item>
    /// <item><description>Geliştirici hata detayları</description></item>
    /// </list>
    /// </para>
    /// <para>
    /// Development ortamında daha detaylı hata bilgileri gösterilirken, 
    /// Production ortamında kullanıcı dostu genel hata mesajları gösterilir.
    /// </para>
    /// </remarks>
    /// <example>
    /// Örnek kullanım:
    /// <code>
    /// var errorModel = new ErrorViewModel 
    /// { 
    ///     RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier 
    /// };
    /// return View(errorModel);
    /// </code>
    /// </example>
    public class ErrorViewModel
    {
        /// <summary>
        /// Hata durumunda oluşturulan benzersiz istek kimlik numarası.
        /// Bu ID hata ayıklama ve izleme için kullanılır.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Bu ID şu kaynaklardan elde edilebilir:
        /// <list type="bullet">
        /// <item><description>Activity.Current?.Id</description></item>
        /// <item><description>HttpContext.TraceIdentifier</description></item>
        /// </list>
        /// </para>
        /// <para>
        /// Nullable olarak tanımlanmıştır çünkü bazı hata durumlarında ID mevcut olmayabilir.
        /// </para>
        /// </remarks>
        /// <value>Hataya ait benzersiz izleme ID'si veya null</value>
        public string? RequestId { get; set; }

        /// <summary>
        /// RequestId'nin görüntülenip görüntülenmeyeceğini belirleyen property.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Bu property şu durumlarda true döner:
        /// <list type="bullet">
        /// <item><description>RequestId değeri mevcutsa</description></item>
        /// <item><description>RequestId boş string değilse</description></item>
        /// </list>
        /// </para>
        /// </remarks>
        /// <value>RequestId null veya boş değilse true, aksi halde false</value>
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
