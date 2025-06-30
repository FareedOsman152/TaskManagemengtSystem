using TaskManagmentSystem.Helpers;
using TaskManagmentSystem.Models;
using TaskManagmentSystem.Repositories.Interfaces;
using TaskManagmentSystem.Srvices;

namespace TaskManagmentSystem.Repositories
{
    public class TeamAppUserRepository : ITeamAppUserRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<TeamAppUserRepository> _logger;

        public TeamAppUserRepository(AppDbContext context, ILogger<TeamAppUserRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<OperationResult> AddAsync(TeamAppUser teamAppUserToAdd)
        {
            if(teamAppUserToAdd is null)
                return OperationResult.Failure("TeamAppUser cannot be null");
            try
            {
                await _context.TeamAppUser.AddAsync(teamAppUserToAdd);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding TeamAppUser with UserId: {UserId} and TeamId: {TeamId}",
                    teamAppUserToAdd.UserId, teamAppUserToAdd.TeamId);
                return OperationResult.Failure("An error occurred while adding the TeamAppUser: " + ex.Message);
            }
            return OperationResult.Success();
        }

        public async Task<OperationResult<TeamAppUser>> GetAsync(string userId, int teamId)
        {
            if (string.IsNullOrEmpty(userId))
                return OperationResult<TeamAppUser>.Failure("User ID cannot be null or empty");

            if (teamId <= 0)
                return OperationResult<TeamAppUser>.Failure("Team ID must be greater than zero");

            var teamAppUser = await _context.TeamAppUser.FindAsync(teamId, userId);
            if (teamAppUser is null)
                return OperationResult<TeamAppUser>.Failure("TeamAppUser not found for the given UserId and TeamId");

            return OperationResult<TeamAppUser>.Success(teamAppUser);
        }

        public async Task<OperationResult<TeamPermissions>> GetPermissionsAsync(string userId, int teamId)
        {
            var teamAppUser = await GetAsync(userId, teamId);
            if (!teamAppUser.Succeeded)
                return OperationResult<TeamPermissions>.Failure(teamAppUser.ErrorMessage);
            if (teamAppUser.Data is null)
                return OperationResult<TeamPermissions>.Failure("TeamAppUser not found");
           
            return OperationResult<TeamPermissions>.Success(teamAppUser.Data.Permissons);
        }

    }
}
