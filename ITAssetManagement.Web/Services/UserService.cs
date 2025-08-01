using ITAssetManagement.Web.Services.Interfaces;
using ITAssetManagement.Web.Models;
using ITAssetManagement.Web.Data.Repositories;
using ITAssetManagement.Web.Data;
using Microsoft.EntityFrameworkCore;

namespace ITAssetManagement.Web.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ApplicationDbContext _context;

        public UserService(IUserRepository userRepository, ApplicationDbContext context)
        {
            _userRepository = userRepository;
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllAsync();
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _userRepository.GetUserWithAssignmentsAsync(id);
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

        public IQueryable<User> SearchUsersQueryable(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return _context.Users.Include(u => u.Assignments);

            searchTerm = searchTerm.ToLower();
            return _context.Users
                .Include(u => u.Assignments)
                .Where(u => 
                    u.FullName.ToLower().Contains(searchTerm) ||
                    u.Email.ToLower().Contains(searchTerm) ||
                    (u.Department != null && u.Department.ToLower().Contains(searchTerm)) ||
                    (u.Position != null && u.Position.ToLower().Contains(searchTerm)) ||
                    (u.Phone != null && u.Phone.Contains(searchTerm)));
        }

        public async Task<IEnumerable<User>> SearchUsersAsync(string searchTerm)
        {
            return await SearchUsersQueryable(searchTerm).ToListAsync();
        }
    }
}
