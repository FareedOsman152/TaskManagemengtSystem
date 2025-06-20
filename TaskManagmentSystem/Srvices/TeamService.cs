using Microsoft.EntityFrameworkCore;
using TaskManagmentSystem.Models;
using TaskManagmentSystem.Repositories.Interfaces;
using TaskManagmentSystem.Srvices.Interfaces;
using TaskManagmentSystem.ViewModels;

namespace TaskManagmentSystem.Srvicese
{
    public class TeamService : ITeamService
    {
        private readonly ITeamRepository _teamRepository;
        private readonly IUserService _userService;

        public TeamService(ITeamRepository teamRepository,IUserService userService)
        {
            _teamRepository = teamRepository;
            _userService = userService;
        }

        public async Task<Team> GetByIdAsync(int id)
        {
            return await _teamRepository.GetByIdAsync(id);
        }

        public async Task<List<TeamsShowViewModel>> GetTeamsForShowAllAsync(string userId)
        {
            var teams = await _teamRepository.GetTeamsOfUserAsync(userId);
            var teamsViewModel = new List<TeamsShowViewModel>();
            if (teams.Count == 0)
                return teamsViewModel;

            return teams.Select(t => new TeamsShowViewModel
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                DateCreated = t.DateCreated,
                AdminId = t.AdminId,
                AdminName = t.Admin.UserName!,
                UserId = userId!
            }).ToList();
        }

        /// <summary>
        /// Adds a new team and sets the creator as admin
        /// </summary>
        public async Task<Team> AddAsync(TeamAddViwModel teamToAdd, string userId)
        {
            if (teamToAdd is null)
                throw new ArgumentNullException($"The {nameof(teamToAdd)} is null");

            if (userId is null)
                throw new ArgumentNullException($"The {nameof(userId)} is null");

            if(!await _userService.IsExistAsync(userId))
                throw new ArgumentException($"The user is not found");

            var team = new Team
            {
                Title = teamToAdd.Title,
                Description = teamToAdd.Description,
                DateCreated = DateTime.Now,
                AdminId = userId
            };

            return  await _teamRepository.AddAsync(team);
        }

        public Task<Team> EditAsync(Team teamToEdit)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteAsync(int id, string userId)
        {
            var team = await GetByIdAsync(id);
            if (team.AdminId == userId)
                await _teamRepository.DeleteAsync(id);
            else throw new ArgumentException("Just Admin can delete the team");
        }

        public async Task<Team> EditAsync(TeamEditViewModel teamToEdit, string userId)
        {
            var team = await GetByIdAsync(teamToEdit.Id);
            if (team.AdminId == userId)
                await _teamRepository.EditAsync(new Team 
                {
                    Id = teamToEdit.Id,
                    Title = teamToEdit.Title,
                    Description=teamToEdit.Description,
                });
            return team;
        }

        public async Task<TeamDeatilsViewModel> GetTeamDetailsInculdeUsersAsync(int id, string userId)
        {
            var team = await GetByIdIncludeUsersAsync(id);
            if (team is null)
                throw new ArgumentException("This team is not found");

            if(!await IsMember(id,userId))
                throw new ArgumentException("You are not a member in this team");

            //var userDetails = await _userService.GetUserDetailsForTeamDetails(userId,team.AdminId);
            var userTasks = team.Users.Select((u) => 
            _userService.GetUserDetailsForTeamDetails(u.Id, team.AdminId))
                .ToList();

            var userDetails = await Task.WhenAll(userTasks);
            return new TeamDeatilsViewModel
            {
                Id = team.Id,
                Title = team.Title,
                Description = team.Description,
                AdminId = team.AdminId,
                UserId = userId,
                Users = userDetails.ToList()
            };
        }

        public async Task<bool> IsMember(int id, string userId)
        {
            var team = await GetByIdIncludeUsersAsync(id);
            return team.Users.FirstOrDefault(u => u.Id == userId) is not null;
        }

        public async Task<bool> IsAdmin(int id, string userId)
        {
            var team = await GetByIdIncludeUsersAsync(id);
            return team.AdminId == userId;
        }

        public async Task<Team> GetByIdIncludeUsersAsync(int id)
        {
            return await _teamRepository.GetByIdIncludeUsersAsync(id);
        }
    }
}
