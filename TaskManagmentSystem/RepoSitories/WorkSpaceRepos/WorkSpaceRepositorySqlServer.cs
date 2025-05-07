using Microsoft.EntityFrameworkCore;
using TaskManagmentSystem.Models;

namespace TaskManagmentSystem.RepoSitories.WorkSpaceRepos
{
    public class WorkSpaceRepositorySqlServer : IWorkSpaceRepository
    {
        private readonly AppDbContext _context;

        public WorkSpaceRepositorySqlServer(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateWorkSpaceAsync(WorkSpace workSpace)
        {
            if (workSpace is null) return false;
            _context.WorkSpaces.Add(workSpace);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteWorkSpaceAsync(int id)
        {
            var workSpace = await _context.WorkSpaces.FindAsync(id);
            if (workSpace is null) return false;
            _context.Remove(workSpace);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<List<WorkSpace>> GetAllWorkSpacesAsync(string userId) =>
            await _context.WorkSpaces.Where(x => x.AppUserId == userId).ToListAsync();

        public async Task<WorkSpace> GetWorkSpaceByIdAsync(int id) =>
            await _context.WorkSpaces.FindAsync(id)?? 
            throw new KeyNotFoundException("WorkSpace not found");

        public async Task<bool> UpdateWorkSpaceAsync(WorkSpace workSpace)
        {
            _context.Update(workSpace);
            int effectedRows = await _context.SaveChangesAsync();
            return effectedRows > 0;
        }
           
        
    }
}
