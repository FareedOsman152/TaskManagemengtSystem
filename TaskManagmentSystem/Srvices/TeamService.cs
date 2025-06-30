using Microsoft.EntityFrameworkCore;
using TaskManagmentSystem.Helpers;
using TaskManagmentSystem.Models;
using TaskManagmentSystem.Repositories.Interfaces;
using TaskManagmentSystem.Srvices;
using TaskManagmentSystem.Srvices.Interfaces;
using TaskManagmentSystem.ViewModels;

namespace TaskManagmentSystem.Srvicese
{
    public class TeamService : ITeamService
    {
        private readonly ITeamRepository _teamRepository;
        private readonly IUserService _userService;
        private readonly ITeamAppUserService _teamAppUserService;

        public TeamService(ITeamRepository teamRepository,IUserService userService, ITeamAppUserService teamAppUserService)
        {
            _teamRepository = teamRepository;
            _userService = userService;
            _teamAppUserService = teamAppUserService;
        }

        public async Task<OperationResult<Team>> GetByIdAsync(int id)
        {
            return await _teamRepository.GetByIdAsync(id);
        }

        public async Task<OperationResult<List<TeamsShowViewModel>>> GetTeamsForShowAllAsync(string userId)
        {
            var teamsResult = await _teamRepository.GetTeamsOfUserAsync(userId);
            if (!teamsResult.Succeeded)
                return OperationResult<List<TeamsShowViewModel>>.Failure(teamsResult.ErrorMessage);

            var teams = teamsResult.Data;
            var teamsViewModel = new List<TeamsShowViewModel>();
            if (teams.Count == 0)
                return OperationResult<List<TeamsShowViewModel>>.Success(teamsViewModel);

            teamsViewModel =  teams.Select(t => new TeamsShowViewModel
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                DateCreated = t.DateCreated,
                AdminId = t.AdminId,
                AdminName = t.Admin.UserName!,
                UserId = userId!
            }).ToList();

            return OperationResult<List<TeamsShowViewModel>>.Success(teamsViewModel);
        }

        /// <summary>
        /// Adds a new team to the system.
        /// the creater is an admin.
        /// add user to the members if team.
        /// </summary>
        /// <param name="teamToAdd"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<OperationResult<Team>> AddAsync(TeamAddViwModel teamToAdd, string userId)
        {
            if (teamToAdd is null)
                return OperationResult<Team>.Failure("Team to add cannot be null");

            if (userId is null)
                return OperationResult<Team>.Failure("User ID cannot be null");

            var userExist = await _userService.IsExistAsync(userId);
            if (!userExist.Succeeded)
                return OperationResult<Team>.Failure("User does not exist");

            var team = new Team
            {
                Title = teamToAdd.Title,
                Description = teamToAdd.Description,
                DateCreated = DateTime.Now,
                AdminId = userId
            };

            var addResult = await _teamRepository.AddAsync(team);
            if (!addResult.Succeeded)
                return OperationResult<Team>.Failure(addResult.ErrorMessage);

            var addUserResult = await _teamAppUserService.AddAsync(userId!, team.Id, TeamPermissions.Admin);
            if (!addUserResult.Succeeded)
            {
                // If adding the user to the team fails, we should delete the team
                await _teamRepository.DeleteAsync(team.Id);
                return OperationResult<Team>.Failure(addUserResult.ErrorMessage);
            }
            return OperationResult<Team>.Success(addResult.Data);
        }
        public async Task<OperationResult> DeleteAsync(int id, string userId)
        {
            var teamresult = await GetByIdAsync(id);
            if (!teamresult.Succeeded)
                return OperationResult.Failure(teamresult.ErrorMessage);

            var team = teamresult.Data;

            if (team.AdminId != userId)
                return OperationResult.Failure("You are not the admin of this team, you cannot delete it.");

            var deleteResult = await _teamRepository.DeleteAsync(id);
            if (!deleteResult.Succeeded)
                return OperationResult.Failure(deleteResult.ErrorMessage);
            return OperationResult.Success();
        }

        public async Task<OperationResult<Team>> EditAsync(TeamEditViewModel teamToEdit, string userId)
        {
            var teamResult = await GetByIdAsync(teamToEdit.Id);
            if (!teamResult.Succeeded)
                return OperationResult<Team>.Failure(teamResult.ErrorMessage);

            var team = teamResult.Data;

            if (team.AdminId == userId)
                return await _teamRepository.EditAsync(new Team 
                {
                    Id = teamToEdit.Id,
                    Title = teamToEdit.Title,
                    Description=teamToEdit.Description,
                });
            return OperationResult<Team>.Failure("You are not the admin of this team, you cannot edit it.");
        }

        public async Task<OperationResult<TeamDeatilsViewModel>> GetTeamDetailsInculdeUsersAsync(int id, string userId)
        {
            var teamResult = await GetByIdIncludeUsersAsync(id);
            if (!teamResult.Succeeded)
                return OperationResult<TeamDeatilsViewModel>.Failure(teamResult.ErrorMessage);
            var team = teamResult.Data;

            var isMemberResult = await IsMember(id, userId);
            if (!isMemberResult.Succeeded)
                return OperationResult<TeamDeatilsViewModel>.Failure(isMemberResult.ErrorMessage);

            
            var userDetails = new List<UserDetailsForTeamViewModel>();
            foreach (var user in team.Users)
            {
                var userDetailsResult = await _userService.GetUserDetailsForTeamDetailsAsync(user.Id, team.AdminId);
                userDetails.Add(userDetailsResult.Data);
            }
            var teamDetails =  new TeamDeatilsViewModel
            {
                Id = team.Id,
                Title = team.Title,
                Description = team.Description,
                AdminId = team.AdminId,
                UserId = userId,
                Users = userDetails
            };
            return OperationResult<TeamDeatilsViewModel>.Success(teamDetails);
        }

        public async Task<OperationResult<bool>> IsMember(int id, string userId)
        {
            var teamResult = await GetByIdIncludeUsersAsync(id);
            if (!teamResult.Succeeded)
                return OperationResult<bool>.Failure(teamResult.ErrorMessage);

            var team = teamResult.Data;
            return OperationResult<bool>.Success(team.Users.Any(u => u.Id == userId));
        }

        public async Task<OperationResult<bool>> IsAdmin(int id, string userId)
        {
            var teamResult = await GetByIdIncludeUsersAsync(id);
            if (!teamResult.Succeeded)
                return OperationResult<bool>.Failure(teamResult.ErrorMessage);

            var team = teamResult.Data;
            return OperationResult<bool>.Success(team.AdminId == userId);
        }

        public async Task<OperationResult<Team>> GetByIdIncludeUsersAsync(int id)
        {
            return await _teamRepository.GetByIdIncludeUsersAsync(id);
        }
    }
}
