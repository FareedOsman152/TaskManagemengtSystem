using TaskManagmentSystem.Models;

namespace TaskManagmentSystem.Repositories.Interfaces
{
    public interface ITeamAppUserRepository
    {
        Task AddAsync(TeamAppUser teamAppUserToAdd);
        Task<TeamAppUser> GetAsync(string userId, int teamId);
        Task<TeamPermissions> GetPermissionsAsync(string userId, int teamId);
    }
}
