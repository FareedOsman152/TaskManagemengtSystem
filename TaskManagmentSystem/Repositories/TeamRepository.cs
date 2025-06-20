using Microsoft.EntityFrameworkCore;
using TaskManagmentSystem.Models;
using TaskManagmentSystem.Repositories.Interfaces;
using TaskManagmentSystem.Srvices;
using TaskManagmentSystem.ViewModels;

namespace TaskManagmentSystem.Repositories
{
    public class TeamRepository : ITeamRepository
    {
        private readonly AppDbContext _context;

        public TeamRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Team> GetByIdAsync(int id)
        {
            var team = await _context.Teams.FindAsync(id);
            if (team is null)
                throw new ArgumentException("The team is not found");

            return team;
        }

        public async Task<List<Team>> GetTeamsOfUserAsync(string userId)
        {
            var userWithTeams = await _context.Users.Include(u=>u.Teams!).ThenInclude(t=>t.Admin)
                .FirstOrDefaultAsync(u=>u.Id==userId);

            if (userWithTeams is null)
                throw new ArgumentException($"There is no user with this Id {userId}");

            return userWithTeams.Teams!;
        }

        public async Task<Team> AddAsync(Team teamToAdd)
        {
            if (teamToAdd is null)
                throw new ArgumentNullException($"The {nameof(teamToAdd)} is null");

            await _context.Teams.AddAsync(teamToAdd);
            await _context.SaveChangesAsync();
            return teamToAdd;
        }

        public async Task<Team> EditAsync(Team teamToEdit)
        {
            Check.IsNull(teamToEdit);

            var team = await _context.Teams.FindAsync(teamToEdit.Id);
            Check.IsNull(team!);

            team.Title = teamToEdit.Title;
            team.Description = teamToEdit.Description;
            await _context.SaveChangesAsync();
            return team;
        }

        public async Task DeleteAsync(int id)
        {
            var team = await _context.Teams.FindAsync(id);
            if (team is null)
                throw new ArgumentException("this team is not found");
            _context.Teams.Remove(team);
            await _context.SaveChangesAsync();
        }

        public async Task<Team> GetByIdIncludeUsersAsync(int id)
        {
            var team = await _context.Teams.Include(t=>t.Users)
                .FirstOrDefaultAsync(t=>t.Id==id);

            if (team is null)
                throw new ArgumentException("The team is not found");

            return team;
        }
    }
}
