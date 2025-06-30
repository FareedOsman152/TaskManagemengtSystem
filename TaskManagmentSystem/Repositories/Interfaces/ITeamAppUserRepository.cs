using TaskManagmentSystem.Helpers;
using TaskManagmentSystem.Models;

namespace TaskManagmentSystem.Repositories.Interfaces
{
    public interface ITeamAppUserRepository
    {
        Task<OperationResult> AddAsync(TeamAppUser teamAppUserToAdd);
        Task<OperationResult<TeamAppUser>> GetAsync(string userId, int teamId);
        Task<OperationResult<TeamPermissions>> GetPermissionsAsync(string userId, int teamId);
    }
}
