using TaskManagmentSystem.Models;

namespace TaskManagmentSystem.Repositories.Interfaces
{
    public interface ITeamRepository
    {
        Task<Team> GetByIdAsync(int id);
        Task<Team> GetByIdIncludeUsersAsync(int id);
        Task<List<Team>> GetTeamsOfUserAsync(string userId);
        Task<Team> AddAsync(Team teamToAdd);
        Task<Team> EditAsync(Team teamToEdit);
        Task DeleteAsync(int id);
    }
}
