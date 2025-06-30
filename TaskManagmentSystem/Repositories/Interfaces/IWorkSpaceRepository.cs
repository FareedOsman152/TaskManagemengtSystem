using TaskManagmentSystem.Helpers;
using TaskManagmentSystem.Models;

namespace TaskManagmentSystem.Repositories.Interfaces
{
    public interface IWorkSpaceRepository
    {
        Task<OperationResult<WorkSpace>> GetByIdAsync(int id);
        Task<OperationResult> CreateAsync(WorkSpace workSpace);
        Task<OperationResult> UpdateAsync(WorkSpace workSpace);
        Task<OperationResult> DeleteAsync(int id);
        Task<OperationResult<List<WorkSpace>>> GetForUserAsync(string userId);
        Task<OperationResult<List<WorkSpace>>> GetForTeamAsync(int teamId);
    }
}
