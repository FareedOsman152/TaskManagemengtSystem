using TaskManagmentSystem.Helpers;
using TaskManagmentSystem.Models;

namespace TaskManagmentSystem.Srvices.Interfaces
{
    public interface ITeamAppUserService
    {
        Task<OperationResult> AddAsync(string userId, int teamId, TeamPermissions permissions);
        Task<OperationResult<TeamAppUser>> GetAsync(string userId, int teamId);
        Task<OperationResult<bool>> IsMemberAsync(string userId, int teamId);
        Task<OperationResult<bool>> IsHasPermissionAsync(string userId, int teamId, TeamPermissions permossionsCheck);
    }
}
