using ITAssetManagement.Web.Services.Interfaces;
using ITAssetManagement.Web.Models;
using ITAssetManagement.Web.Data.Repositories;

namespace ITAssetManagement.Web.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllAsync();
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _userRepository.GetByIdAsync(id);
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _userRepository.GetUserByEmailAsync(email);
        }

        public async Task<IEnumerable<User>> GetUsersByDepartmentAsync(string department)
        {
            return await _userRepository.GetUsersByDepartmentAsync(department);
        }

        public async Task<bool> CreateUserAsync(User user)
        {
            if (user == null)
                return false;

            // Email benzersizlik kontrolü
            var existingUser = await _userRepository.GetUserByEmailAsync(user.Email);
            if (existingUser != null)
                return false;

            await _userRepository.AddAsync(user);
            return await _userRepository.SaveChangesAsync();
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            if (user == null)
                return false;

            _userRepository.Update(user);
            return await _userRepository.SaveChangesAsync();
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                return false;

            _userRepository.Remove(user);
            return await _userRepository.SaveChangesAsync();
        }
    }
}
