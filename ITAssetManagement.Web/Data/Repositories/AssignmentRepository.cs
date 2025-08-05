using ITAssetManagement.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace ITAssetManagement.Web.Data.Repositories
{
    public class AssignmentRepository : GenericRepository<Assignment>, IAssignmentRepository
    {
        private readonly new ApplicationDbContext _context;

        public AssignmentRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable<Assignment> GetAllWithDetailsQueryable()
        {
            return _context.Assignments
                .Include(a => a.User)
                .Include(a => a.Laptop)
                .OrderByDescending(a => a.AssignmentDate)
                .AsNoTracking();
        }

        public async Task<IEnumerable<Assignment>> GetAllWithDetailsAsync()
        {
            return await _context.Assignments
                .Include(a => a.User)
                .Include(a => a.Laptop)
                .ToListAsync();
        }

        public async Task<Assignment?> GetByIdWithDetailsAsync(int id)
        {
            return await _context.Assignments
                .Include(a => a.User)
                .Include(a => a.Laptop)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public void Delete(Assignment entity)
        {
            _context.Set<Assignment>().Remove(entity);
        }

        public new async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
