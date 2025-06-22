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

        public async Task AddAsync(string userId, int teamId)
        {
            Check.IsNull(userId);

            await _teamAppUserRepository
                .AddAsync(new TeamAppUser { TeamId = teamId, UserId = userId });
        }

        public async Task<bool> IsHasPermissionAsync(string userId, int teamId, TeamPermissions permossionsCheck)
        {
            var permissions = await _teamAppUserRepository.GetPermissionsAsync(userId,teamId);
            return permissions  == TeamPermissions.Admin || (permissions & permossionsCheck) == permossionsCheck ;
        }
    }
}
