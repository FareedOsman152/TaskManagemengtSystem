using TaskManagmentSystem.Helpers;
using TaskManagmentSystem.Models;
using TaskManagmentSystem.ViewModels;

namespace TaskManagmentSystem.Srvices.Interfaces
{
    public interface IUserService
    {
        Task<OperationResult<AppUser>> GetByIdAsync(string userId);
        Task<OperationResult<AppUser>> GetIncludeTeamsAsync(string userId);
        Task<OperationResult<AppUser>> GetByUserNameAsync(string userName);
        Task<OperationResult> IsExistAsync(string userId);
        Task<OperationResult<UserDetailsForTeamViewModel>> GetUserDetailsForTeamDetailsAsync(string userId, string adminId);
    }
}
