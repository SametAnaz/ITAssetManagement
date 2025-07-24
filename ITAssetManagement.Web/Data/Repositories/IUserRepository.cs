using ITAssetManagement.Web.Models;

namespace ITAssetManagement.Web.Data.Repositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User?> GetUserByEmailAsync(string email);
        Task<IEnumerable<User>> GetUsersByDepartmentAsync(string department);
        Task<User?> GetUserWithAssignmentsAsync(int id);
    }
}
