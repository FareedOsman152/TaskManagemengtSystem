using TaskManagmentSystem.Models;
using TaskManagmentSystem.Repositories.Interfaces;
using TaskManagmentSystem.Srvices.Interfaces;

namespace TaskManagmentSystem.Srvices
{
    public class TeamAppUserService : ITeamAppUserService
    {
        private readonly ITeamAppUserRepository _teamAppUserRepository;
        public TeamAppUserService(ITeamAppUserRepository teamAppUserRepository)
        {
            _teamAppUserRepository = teamAppUserRepository;
        }

        /// <summary>
        /// Adds a user to a team with specified permissions.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="teamId"></param>
        /// <param name="permissions"></param>
        /// <returns></returns>
        public async Task AddAsync(string userId, int teamId, TeamPermissions permissions)
        {
            Check.IsNull(userId);

            await _teamAppUserRepository
                .AddAsync(new TeamAppUser { TeamId = teamId, UserId = userId, Permissons = permissions });
        }

        public async Task<TeamAppUser> GetAsync(string userId, int teamId)
        {
            return await _teamAppUserRepository.GetAsync(userId, teamId);
        }
        public async Task<bool> IsMemberAsync(string userId, int teamId)
        {
            return await GetAsync(userId, teamId) is not null;
        }

        public async Task<bool> IsHasPermissionAsync(string userId, int teamId, TeamPermissions permossionsCheck)
        {
            var permissions = await _teamAppUserRepository.GetPermissionsAsync(userId,teamId);
            return permissions  == TeamPermissions.Admin || (permissions & permossionsCheck) == permossionsCheck ;
        }
    }
}
