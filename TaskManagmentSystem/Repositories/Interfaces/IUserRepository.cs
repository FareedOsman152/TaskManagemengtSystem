using TaskManagmentSystem.Helpers;
using TaskManagmentSystem.Models;

namespace TaskManagmentSystem.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<OperationResult<AppUser>> GetByIdAsync(string userId);
        Task<OperationResult<AppUser>> GetByIdIncludeTeamsAsync(string userId);
        Task<OperationResult<AppUser>> GetByUserNameAsync(string userName);
        Task<OperationResult> IsExistAsync(string userId);
    }
}
