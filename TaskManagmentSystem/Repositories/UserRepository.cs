using Microsoft.EntityFrameworkCore;
using TaskManagmentSystem.Helpers;
using TaskManagmentSystem.Models;
using TaskManagmentSystem.Repositories.Interfaces;

namespace TaskManagmentSystem.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<OperationResult<AppUser>> GetByIdAsync(string userId)
        {
             var user = await _context.Users.FindAsync(userId);
            if(user is null)
                return OperationResult<AppUser>.Failure("User not found");

            return OperationResult<AppUser>.Success(user);
        }

        public async Task<OperationResult<AppUser>> GetByIdIncludeTeamsAsync(string userId)
        {
            var user = await _context.Users.Include(u=>u.Teams).ThenInclude(t=>t.Admin).FirstOrDefaultAsync(u => u.Id == userId);
            if(user is null)
                return OperationResult<AppUser>.Failure("User not found");
            return OperationResult<AppUser>.Success(user);
        }

        public async Task<OperationResult<AppUser>> GetByUserNameAsync(string userName)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName);
            if (user is null)
                return OperationResult<AppUser>.Failure("User not found");

            return OperationResult<AppUser>.Success(user);
        }

        public async Task<OperationResult> IsExistAsync(string userId)
        {
            var userResult  = await GetByIdAsync(userId);
            if (userResult.Succeeded)
                return OperationResult.Success();

            return OperationResult.Failure(userResult.ErrorMessage);
        }
    }
}
