using Microsoft.EntityFrameworkCore;
using TaskManagmentSystem.Helpers;
using TaskManagmentSystem.Models;
using TaskManagmentSystem.Repositories.Interfaces;
using TaskManagmentSystem.Srvices.Interfaces;

namespace TaskManagmentSystem.Repositories
{
    public class WorkSpaceRepository : IWorkSpaceRepository
    {
        private readonly AppDbContext _context;
        private readonly ITeamService _teamService;
        private readonly ILogger<WorkSpaceRepository> _logger;

        public WorkSpaceRepository(AppDbContext context, ITeamService teamService, ILogger<WorkSpaceRepository> logger)
        {
            _context = context;
            _teamService = teamService;
            _logger = logger;
        }

        public async Task<OperationResult<WorkSpace>> GetByIdAsync(int id)
        {
            var workSpace = await _context.WorkSpaces.FindAsync(id);
            if (workSpace == null)
                return OperationResult<WorkSpace>.Failure("WorkSpace not found");
            return OperationResult<WorkSpace>.Success(workSpace);
        }
        public async Task<OperationResult> CreateAsync(WorkSpace workSpace)
        {
            _context.WorkSpaces.Add(workSpace);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating WorkSpace with Title: {Title}", workSpace.Title);
                return OperationResult.Failure("An error occurred while creating the WorkSpace: " + ex.Message);
            }
            
            return OperationResult.Success();
        }
        public async Task<OperationResult> UpdateAsync(WorkSpace workSpace)
        {
            _context.WorkSpaces.Update(workSpace);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating WorkSpace with Title: {Title}", workSpace.Title);
                return OperationResult.Failure("An error occurred while updating the WorkSpace: " + ex.Message);
            }
            return OperationResult.Success();
        }
        public async Task<OperationResult> DeleteAsync(int id)
        {
            var workSpace = await _context.WorkSpaces.FindAsync(id);
            if (workSpace == null)
                return OperationResult.Failure("WorkSpace not found");
            _context.WorkSpaces.Remove(workSpace);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while Deleting WorkSpace with Title: {Title}", workSpace.Title);
                return OperationResult.Failure("An error occurred while Deleting the WorkSpace: " + ex.Message);
            }
            return OperationResult.Success();
        }
        public async Task<OperationResult<List<WorkSpace>>> GetForUserAsync(string userId)
        {
            var workSpaces = await _context.WorkSpaces
                .Where(ws => ws.AppUserId == userId)
                .ToListAsync();
            return OperationResult<List<WorkSpace>>.Success(workSpaces);
        }

        public async Task<OperationResult<List<WorkSpace>>> GetForTeamAsync(int teamId)
        {
            var teamResult = await _teamService.GetByIdAsync(teamId);
            if(!teamResult.Succeeded)
                return OperationResult<List<WorkSpace>>.Failure("Team not found");

            var workSpaces = await _context.WorkSpaces
                .Where(ws => ws.TeamId == teamId)
                .ToListAsync();
            return OperationResult<List<WorkSpace>>.Success(workSpaces);
        }
    }
}
