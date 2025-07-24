using ITAssetManagement.Web.Models;

namespace ITAssetManagement.Web.Services.Interfaces
{
    public interface IAssignmentService
    {
        Task<IEnumerable<Assignment>> GetAllAssignmentsAsync();
        Task<Assignment?> GetAssignmentByIdAsync(int id);
        Task CreateAssignmentAsync(Assignment assignment);
        Task UpdateAssignmentAsync(Assignment assignment);
        Task DeleteAssignmentAsync(int id);
    }
}
