using TaskManagmentSystem.Helpers;
using TaskManagmentSystem.Models;
using TaskManagmentSystem.Repositories.Interfaces;
using TaskManagmentSystem.Srvices.Interfaces;

namespace TaskManagmentSystem.Srvices
{
    public class TeamAppUserService : ITeamAppUserService
    {
        private readonly ITeamAppUserRepository _teamAppUserRepository;
        private readonly IUserService _userService;
        private readonly ITeamService _teamService;
        private readonly ILogger<TeamAppUserService> _logger;
        public TeamAppUserService(ITeamAppUserRepository teamAppUserRepository, ILogger<TeamAppUserService> logger, IUserService userService)
        {
            _teamAppUserRepository = teamAppUserRepository;
            _logger = logger;
            _userService = userService;
        }

        /// <summary>
        /// Adds a user to a team with specified permissions.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="teamId"></param>
        /// <param name="permissions"></param>
        /// <returns></returns>
        public async Task<OperationResult> AddAsync(string userId, int teamId, TeamPermissions permissions)
        {
            if (string.IsNullOrEmpty(userId))
                return OperationResult.Failure("User ID cannot be null or empty");

            if (teamId <= 0)
                return OperationResult.Failure("Team ID must be greater than zero");

            var isUserExistResult = await _userService.IsExistAsync(userId);
            if (!isUserExistResult.Succeeded)
                return OperationResult.Failure("User does not exist");

            var isTeamExistResult = await _teamService.GetByIdAsync(teamId);
            if (!isTeamExistResult.Succeeded)
                return OperationResult.Failure("Team does not exist");

            var isMemberResult = await IsMemberAsync(userId, teamId);
            if (isMemberResult.Succeeded)
                return OperationResult.Failure("User is already a member of the team");

            var addResult = await _teamAppUserRepository.AddAsync(new TeamAppUser
            {
                UserId = userId,
                TeamId = teamId,
                Permissons = permissions
            });
            if (!addResult.Succeeded)
                return OperationResult.Failure(addResult.ErrorMessage);

            return OperationResult.Success();
        }

        public async Task<OperationResult<TeamAppUser>> GetAsync(string userId, int teamId)
        {
            return await _teamAppUserRepository.GetAsync(userId, teamId);
        }
        public async Task<OperationResult<bool>> IsMemberAsync(string userId, int teamId)
        {
            if (string.IsNullOrEmpty(userId))
                return OperationResult<bool>.Failure("User ID cannot be null or empty");

            if (teamId <= 0)
                return OperationResult<bool>.Failure("Team ID must be greater than zero");

            var getResult = await GetAsync(userId, teamId);
            return getResult.Succeeded && getResult.Data != null
                ? OperationResult<bool>.Success(true)
                : OperationResult<bool>.Success(false);

        }

        public async Task<OperationResult<bool>> IsHasPermissionAsync(string userId, int teamId, TeamPermissions permossionsCheck)
        {
            var permissions = await _teamAppUserRepository.GetPermissionsAsync(userId,teamId);
            var isHasPermission = permissions.Succeeded && permissions.Data != null;
            if (!isHasPermission)
                return OperationResult<bool>.Success(false);

            if (permissions.Data == TeamPermissions.Admin || (permissions.Data & permossionsCheck) == permossionsCheck)
                return OperationResult<bool>.Success(true);

            return OperationResult<bool>.Success(false);
        }
    }
}
