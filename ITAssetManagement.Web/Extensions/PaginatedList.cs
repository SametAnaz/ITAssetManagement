using Microsoft.EntityFrameworkCore;

namespace ITAssetManagement.Web.Extensions
{
    /// <summary>
    /// Veri listelerinin sayfalama işlemleri için kullanılan generic liste sınıfı.
    /// Entity Framework sorguları için sayfalama yapar ve sayfa navigasyonu için gerekli özellikleri sağlar.
    /// </summary>
    /// <typeparam name="T">Liste elemanlarının tipi. Herhangi bir entity veya model olabilir.</typeparam>
    public class PaginatedList<T> : List<T>
    {
        /// <summary>
        /// Mevcut sayfa numarası. 1'den başlar.
        /// </summary>
        /// <value>Aktif sayfanın numarası</value>
        public int PageIndex { get; private set; }

        /// <summary>
        /// Toplam sayfa sayısı. Toplam öğe sayısı ve sayfa başına öğe sayısına göre hesaplanır.
        /// </summary>
        /// <value>Mevcut veri kümesi için toplam sayfa sayısı</value>
        public int TotalPages { get; private set; }

        /// <summary>
        /// Veri kümesindeki toplam öğe sayısı.
        /// </summary>
        /// <value>Tüm kayıtların toplam sayısı</value>
        public int TotalItems { get; private set; }

        /// <summary>
        /// PaginatedList sınıfının constructor metodu.
        /// Sayfalama parametrelerine göre yeni bir sayfalanmış liste oluşturur.
        /// </summary>
        /// <param name="items">Mevcut sayfada gösterilecek öğeler listesi</param>
        /// <param name="count">Veri kümesindeki toplam kayıt sayısı</param>
        /// <param name="pageIndex">Gösterilecek sayfa numarası (1'den başlar)</param>
        /// <param name="pageSize">Her sayfada gösterilecek öğe sayısı</param>
        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            TotalItems = count;

            AddRange(items);
        }

        /// <summary>
        /// Önceki sayfanın mevcut olup olmadığını belirten özellik.
        /// Mevcut sayfa 1'den büyükse true döner.
        /// </summary>
        /// <value>Önceki sayfa varsa true, yoksa false</value>
        public bool HasPreviousPage => PageIndex > 1;

        /// <summary>
        /// Sonraki sayfanın mevcut olup olmadığını belirten özellik.
        /// Mevcut sayfa toplam sayfa sayısından küçükse true döner.
        /// </summary>
        /// <value>Sonraki sayfa varsa true, yoksa false</value>
        public bool HasNextPage => PageIndex < TotalPages;

        /// <summary>
        /// Verilen IQueryable kaynağından asenkron olarak sayfalanmış liste oluşturur.
        /// Entity Framework sorguları için optimize edilmiştir.
        /// </summary>
        /// <param name="source">Sayfalanacak veri kaynağı (IQueryable)</param>
        /// <param name="pageIndex">İstenen sayfa numarası (1'den başlar)</param>
        /// <param name="pageSize">Sayfa başına gösterilecek öğe sayısı</param>
        /// <returns>Asenkron olarak sayfalanmış liste</returns>
        /// <example>
        /// Kullanım örneği:
        /// <code>
        /// var query = context.Users.AsQueryable();
        /// var pagedList = await PaginatedList&lt;User&gt;.CreateAsync(query, pageNumber, pageSize);
        /// </code>
        /// </example>
        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PaginatedList<T>(items, count, pageIndex, pageSize);
        }
    }
}
