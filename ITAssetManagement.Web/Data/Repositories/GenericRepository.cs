using ITAssetManagement.Web.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ITAssetManagement.Web.Data.Repositories
{
    /// <summary>
    /// Genel repository sınıfı implementasyonu
    /// </summary>
    /// <typeparam name="T">Entity tipi</typeparam>
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        /// <summary>
        /// Veritabanı bağlamı
        /// </summary>
        protected readonly ApplicationDbContext _context;

        /// <summary>
        /// Entity için DbSet
        /// </summary>
        protected readonly DbSet<T> _dbSet;

        /// <summary>
        /// GenericRepository constructor
        /// </summary>
        /// <param name="context">Veritabanı bağlamı</param>
        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        /// <summary>
        /// ID'ye göre entity getirir
        /// </summary>
        /// <param name="id">Entity ID</param>
        /// <returns>Bulunan entity veya null</returns>
        public async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        /// <summary>
        /// Tüm entity'leri getirir
        /// </summary>
        /// <returns>Entity listesi</returns>
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        /// <summary>
        /// Belirtilen koşula göre entity'leri filtreler
        /// </summary>
        /// <param name="predicate">Filtreleme koşulu</param>
        /// <returns>Filtrelenmiş entity listesi</returns>
        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        /// <summary>
        /// Yeni bir entity ekler
        /// </summary>
        /// <param name="entity">Eklenecek entity</param>
        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        /// <summary>
        /// Birden fazla entity ekler
        /// </summary>
        /// <param name="entities">Eklenecek entity'lerin koleksiyonu</param>
        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }

        /// <summary>
        /// Var olan bir entity'yi günceller
        /// </summary>
        /// <param name="entity">Güncellenecek entity</param>
        public void Update(T entity)
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        /// <summary>
        /// Bir entity'yi siler
        /// </summary>
        /// <param name="entity">Silinecek entity</param>
        public void Remove(T entity)
        {
            _dbSet.Remove(entity);
        }

        /// <summary>
        /// Birden fazla entity'yi siler
        /// </summary>
        /// <param name="entities">Silinecek entity'lerin koleksiyonu</param>
        public void RemoveRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        /// <summary>
        /// Değişiklikleri kaydeder
        /// </summary>
        /// <returns>Kaydetme işlemi başarılıysa true, aksi halde false</returns>
        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
