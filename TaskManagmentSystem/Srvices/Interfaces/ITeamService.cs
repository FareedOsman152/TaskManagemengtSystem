using TaskManagmentSystem.Helpers;
using TaskManagmentSystem.Models;
using TaskManagmentSystem.ViewModels;

namespace TaskManagmentSystem.Srvices.Interfaces
{
    public interface ITeamService
    {
        Task<OperationResult<Team>> GetByIdAsync(int id);
        Task<OperationResult<Team>> GetByIdIncludeUsersAsync(int id);
        Task<OperationResult<List<TeamsShowViewModel>>> GetTeamsForShowAllAsync(string userId);
        Task<OperationResult<TeamDeatilsViewModel>> GetTeamDetailsInculdeUsersAsync(int id,string userId);
        Task<OperationResult<Team>> AddAsync(TeamAddViwModel teamToAdd, string userId);
        Task<OperationResult<Team>> EditAsync(TeamEditViewModel teamToEdit, string userId);
        Task<OperationResult> DeleteAsync(int id, string userId);
        Task <OperationResult<bool>> IsMember(int id, string userId);
        Task<OperationResult<bool>> IsAdmin(int id, string userId);
    }
}
