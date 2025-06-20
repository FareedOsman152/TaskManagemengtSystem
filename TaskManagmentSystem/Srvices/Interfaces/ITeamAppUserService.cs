using TaskManagmentSystem.Models;

namespace TaskManagmentSystem.Srvices.Interfaces
{
    public interface ITeamAppUserService
    {
        Task AddAsync(string userId, int teamId);
    }
}
