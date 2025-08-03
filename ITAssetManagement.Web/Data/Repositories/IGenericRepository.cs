using System.Linq.Expressions;

namespace ITAssetManagement.Web.Data.Repositories
{
    /// <summary>
    /// Genel repository arayüzü
    /// </summary>
    /// <typeparam name="T">Entity tipi</typeparam>
    public interface IGenericRepository<T> where T : class
    {
        /// <summary>
        /// ID'ye göre entity getirir
        /// </summary>
        /// <param name="id">Entity ID</param>
        /// <returns>Bulunan entity veya null</returns>
        Task<T?> GetByIdAsync(int id);

        /// <summary>
        /// Tüm entity'leri getirir
        /// </summary>
        /// <returns>Entity listesi</returns>
        Task<IEnumerable<T>> GetAllAsync();

        /// <summary>
        /// Belirtilen koşula göre entity'leri filtreler
        /// </summary>
        /// <param name="predicate">Filtreleme koşulu</param>
        /// <returns>Filtrelenmiş entity listesi</returns>
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Yeni bir entity ekler
        /// </summary>
        /// <param name="entity">Eklenecek entity</param>
        Task AddAsync(T entity);

        /// <summary>
        /// Birden fazla entity'yi toplu olarak ekler
        /// </summary>
        /// <param name="entities">Eklenecek entity'ler</param>
        Task AddRangeAsync(IEnumerable<T> entities);

        /// <summary>
        /// Entity'yi günceller
        /// </summary>
        /// <param name="entity">Güncellenecek entity</param>
        void Update(T entity);

        /// <summary>
        /// Entity'yi siler
        /// </summary>
        /// <param name="entity">Silinecek entity</param>
        void Remove(T entity);

        /// <summary>
        /// Birden fazla entity'yi toplu olarak siler
        /// </summary>
        /// <param name="entities">Silinecek entity'ler</param>
        void RemoveRange(IEnumerable<T> entities);

        /// <summary>
        /// Yapılan değişiklikleri veritabanına kaydeder
        /// </summary>
        /// <returns>İşlem başarılı ise true</returns>
        Task<bool> SaveChangesAsync();
    }
}
