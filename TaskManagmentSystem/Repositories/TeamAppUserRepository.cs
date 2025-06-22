using TaskManagmentSystem.Models;
using TaskManagmentSystem.Repositories.Interfaces;
using TaskManagmentSystem.Srvices;

namespace TaskManagmentSystem.Repositories
{
    public class TeamAppUserRepository : ITeamAppUserRepository
    {
        private readonly AppDbContext _context;

        public TeamAppUserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(TeamAppUser teamAppUserToAdd)
        {
            Check.IsNull(teamAppUserToAdd);
            // to edit 
            teamAppUserToAdd.Permissons = TeamPermissions.Admin;
            await _context.TeamAppUser.AddAsync(teamAppUserToAdd);
            await _context.SaveChangesAsync();
        }

        public async Task<TeamAppUser> GetAsync(string userId, int teamId)
        {
            return await _context.TeamAppUser.FindAsync(userId, teamId);
        }

        public async Task<TeamPermissions> GetPermissionsAsync(string userId, int teamId)
        {
            var teamAppUser = await _context.TeamAppUser.FindAsync(teamId, userId);
            Check.IsNull(teamAppUser!);
            return teamAppUser!.Permissons;
        }

    }
}
