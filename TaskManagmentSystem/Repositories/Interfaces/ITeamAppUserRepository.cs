using TaskManagmentSystem.Models;

namespace TaskManagmentSystem.Repositories.Interfaces
{
    public interface ITeamAppUserRepository
    {
        Task AddAsync(TeamAppUser teamAppUserToAdd);
    }
}
