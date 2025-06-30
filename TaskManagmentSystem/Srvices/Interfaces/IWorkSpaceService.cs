using TaskManagmentSystem.Helpers;
using TaskManagmentSystem.Models;
using TaskManagmentSystem.ViewModels;

namespace TaskManagmentSystem.Srvices.Interfaces
{
    public interface IWorkSpaceService
    {
        Task<OperationResult<WorkSpace>> GetByIdAsync(int id);
        Task<OperationResult> CreateAsync(WorkSpaceViewModel workSpaceToCreate, string userId);
        Task<OperationResult> UpdateAsync(WorkSpaceForEditViewModel workSpaceToUpdate);
        Task<OperationResult> DeleteAsync(int id);
        Task<OperationResult<List<WorkSpace>>> GetForUserAsync(string userId);
        Task<OperationResult<List<WorkSpace>>> GetForTeamAsync(int teamId, string userId);
        Task<OperationResult<FullDataWorkSpaceForTeamViewModel>> GetForTeamShowAsync(int teamId, string userId);
    }
}
