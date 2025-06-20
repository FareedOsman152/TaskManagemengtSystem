using TaskManagmentSystem.Models;
using TaskManagmentSystem.ViewModels;

namespace TaskManagmentSystem.Srvices.Interfaces
{
    public interface ITeamService
    {
        Task<Team> GetByIdAsync(int id);
        Task<Team> GetByIdIncludeUsersAsync(int id);
        Task<List<TeamsShowViewModel>> GetTeamsForShowAllAsync(string userId);
        Task<TeamDeatilsViewModel> GetTeamDetailsInculdeUsersAsync(int id,string userId);
        Task<Team> AddAsync(TeamAddViwModel teamToAdd, string userId);
        Task<Team> EditAsync(TeamEditViewModel teamToEdit, string userId);
        Task DeleteAsync(int id, string userId);
        Task <bool> IsMember(int id, string userId);
        Task <bool> IsAdmin(int id, string userId);
    }
}
