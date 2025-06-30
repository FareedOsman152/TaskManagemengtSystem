using TaskManagmentSystem.Helpers;
using TaskManagmentSystem.Models;

namespace TaskManagmentSystem.Repositories.Interfaces
{
    public interface ITeamRepository
    {
        Task<OperationResult<Team>> GetByIdAsync(int id);
        Task<OperationResult<Team>> GetByIdIncludeUsersAsync(int id);
        Task<OperationResult<List<Team>>> GetTeamsOfUserAsync(string userId);
        Task<OperationResult<Team>> AddAsync(Team teamToAdd);
        Task<OperationResult<Team>> EditAsync(Team teamToEdit);
        Task<OperationResult> DeleteAsync(int id);
    }
}
