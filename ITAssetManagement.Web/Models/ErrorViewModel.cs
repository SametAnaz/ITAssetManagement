namespace ITAssetManagement.Web.Models
{
    /// <summary>
    /// Hata sayfalarında kullanılan görünüm modeli
    /// </summary>
    public class ErrorViewModel
    {
        /// <summary>
        /// Hata isteğinin benzersiz kimlik numarası
        /// </summary>
        public string? RequestId { get; set; }

        /// <summary>
        /// RequestId'nin görüntülenip görüntülenmeyeceğini belirler
        /// </summary>
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
