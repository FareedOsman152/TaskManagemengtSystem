using TaskManagmentSystem.Models;

namespace TaskManagmentSystem.Srvices.Interfaces
{
    public interface ITeamAppUserService
    {
        Task AddAsync(string userId, int teamId, TeamPermissions permissions);
        Task<TeamAppUser> GetAsync(string userId, int teamId);
        Task<bool> IsMemberAsync(string userId, int teamId);
        Task<bool> IsHasPermissionAsync(string userId, int teamId, TeamPermissions permossionsCheck);
    }
}
