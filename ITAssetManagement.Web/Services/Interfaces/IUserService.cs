using ITAssetManagement.Web.Models;

namespace ITAssetManagement.Web.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User?> GetUserByIdAsync(int id);
        Task<User?> GetUserByEmailAsync(string email);
        Task<IEnumerable<User>> GetUsersByDepartmentAsync(string department);
        Task<bool> CreateUserAsync(User user);
        Task<bool> UpdateUserAsync(User user);
        Task<bool> DeleteUserAsync(int id);
    }
}
