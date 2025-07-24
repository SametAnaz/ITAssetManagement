using ITAssetManagement.Web.Models;

namespace ITAssetManagement.Web.Data.Repositories
{
    public interface IAssignmentRepository : IGenericRepository<Assignment>
    {
        Task<IEnumerable<Assignment>> GetAllWithDetailsAsync();
        Task<Assignment?> GetByIdWithDetailsAsync(int id);
        void Delete(Assignment entity);
        new Task SaveChangesAsync();
    }
}
