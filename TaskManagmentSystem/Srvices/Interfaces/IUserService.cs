using TaskManagmentSystem.Models;
using TaskManagmentSystem.ViewModels;

namespace TaskManagmentSystem.Srvices.Interfaces
{
    public interface IUserService
    {
        Task<AppUser> GetByIdAsync(string userId);
        Task<AppUser> GetByUserNameAsync(string userName);
        Task<bool> IsExistAsync(string userId);
        Task<UserDetailsForTeamViewModel> GetUserDetailsForTeamDetails(string userId, string adminId);
    }
}
