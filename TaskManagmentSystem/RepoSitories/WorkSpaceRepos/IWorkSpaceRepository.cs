using TaskManagmentSystem.Models;

namespace TaskManagmentSystem.RepoSitories.WorkSpaceRepos
{
    public interface IWorkSpaceRepository
    {
        Task<List<WorkSpace>> GetAllWorkSpacesAsync(string userId);
        Task<WorkSpace> GetWorkSpaceByIdAsync(int id);
        Task<bool> CreateWorkSpaceAsync(WorkSpace workSpace);
        Task<bool> UpdateWorkSpaceAsync(WorkSpace workSpace);
        Task<bool> DeleteWorkSpaceAsync(int id);
    }
}
