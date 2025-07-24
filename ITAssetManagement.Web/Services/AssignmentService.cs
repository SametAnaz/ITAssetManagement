using ITAssetManagement.Web.Data.Repositories;
using ITAssetManagement.Web.Models;
using ITAssetManagement.Web.Services.Interfaces;

namespace ITAssetManagement.Web.Services
{
    public class AssignmentService : IAssignmentService
    {
        private readonly IAssignmentRepository _assignmentRepository;

        public AssignmentService(IAssignmentRepository assignmentRepository)
        {
            _assignmentRepository = assignmentRepository;
        }

        public async Task<IEnumerable<Assignment>> GetAllAssignmentsAsync()
        {
            return await _assignmentRepository.GetAllWithDetailsAsync();
        }

        public async Task<Assignment?> GetAssignmentByIdAsync(int id)
        {
            return await _assignmentRepository.GetByIdWithDetailsAsync(id);
        }

        public async Task CreateAssignmentAsync(Assignment assignment)
        {
            await _assignmentRepository.AddAsync(assignment);
            await _assignmentRepository.SaveChangesAsync();
        }

        public async Task UpdateAssignmentAsync(Assignment assignment)
        {
            _assignmentRepository.Update(assignment);
            await _assignmentRepository.SaveChangesAsync();
        }

        public async Task DeleteAssignmentAsync(int id)
        {
            var assignment = await _assignmentRepository.GetByIdAsync(id);
            if (assignment != null)
            {
                _assignmentRepository.Delete(assignment);
                await _assignmentRepository.SaveChangesAsync();
            }
        }
    }
}
