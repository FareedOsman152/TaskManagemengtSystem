using TaskManagmentSystem.Models;
using TaskManagmentSystem.Repositories.Interfaces;

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
            await _context.TeamAppUser.AddAsync(teamAppUserToAdd);
            await _context.SaveChangesAsync();
        }
    }
}
