using Microsoft.EntityFrameworkCore;
using TaskManagmentSystem.Helpers;
using TaskManagmentSystem.Models;
using TaskManagmentSystem.Repositories.Interfaces;
using TaskManagmentSystem.Srvices;
using TaskManagmentSystem.Srvices.Interfaces;
using TaskManagmentSystem.ViewModels;

namespace TaskManagmentSystem.Repositories
{
    public class TeamRepository : ITeamRepository
    {
        private readonly AppDbContext _context;
        private readonly IUserService _userService;

        public TeamRepository(AppDbContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        public async Task<OperationResult<Team>> GetByIdAsync(int id)
        {
            var team = await _context.Teams.FindAsync(id);
            if (team is null)
                return OperationResult<Team>.Failure("Team not found");

            return OperationResult<Team>.Success(team);
        }

        public async Task<OperationResult<List<Team>>> GetTeamsOfUserAsync(string userId)
        {
            var userResult = await _userService.GetByIdAsync(userId);
            if(!userResult.Succeeded)
                return OperationResult<List<Team>>.Failure(userResult.ErrorMessage);
            
            var user = userResult.Data;
            var userWithTeamsResult = await _userService.GetIncludeTeamsAsync(userId);
            if(!userWithTeamsResult.Succeeded)
                return OperationResult<List<Team>>.Failure(userWithTeamsResult.ErrorMessage);

            var userWithTeams = userWithTeamsResult.Data;

            return OperationResult<List<Team>>.Success(userWithTeams.Teams);
        }

        public async Task<OperationResult<Team>> AddAsync(Team teamToAdd)
        {
            if (teamToAdd is null)
                return OperationResult<Team>.Failure("The team to add is null");
            try
            {
                await _context.Teams.AddAsync(teamToAdd);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return OperationResult<Team>.Failure($"An error occurred while adding the team: {ex.Message}");
            }
            return OperationResult<Team>.Success(teamToAdd);
        }

        public async Task<OperationResult<Team>> EditAsync(Team teamToEdit)
        {
            if (teamToEdit is null)
                return OperationResult<Team>.Failure("The team to edit is null");

            var teamResult = await GetByIdAsync(teamToEdit.Id);
            if (!teamResult.Succeeded)
                return OperationResult<Team>.Failure(teamResult.ErrorMessage);

            var team = teamResult.Data;

            team.Title = teamToEdit.Title;
            team.Description = teamToEdit.Description;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return OperationResult<Team>.Failure($"An error occurred while updating the team: {ex.Message}");
            }
           
            return OperationResult<Team>.Success(team);
        }

        public async Task<OperationResult> DeleteAsync(int id)
        {
            var teamresult = await GetByIdAsync(id);
            if (!teamresult.Succeeded)
                return OperationResult.Failure(teamresult.ErrorMessage);
            var team = teamresult.Data;
            try
            {
                _context.Teams.Remove(team);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return OperationResult.Failure($"An error occurred while deleting the team: {ex.Message}");
            }
            return OperationResult.Success();
        }

        public async Task<OperationResult<Team>> GetByIdIncludeUsersAsync(int id)
        {
            var team = await _context.Teams.Include(t=>t.Users)
                .FirstOrDefaultAsync(t=>t.Id==id);

            if (team is null)
                return OperationResult<Team>.Failure("Team not found");

            return OperationResult<Team>.Success(team);
        }

    }
}
