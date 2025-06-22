using TaskManagmentSystem.Models;

namespace TaskManagmentSystem.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<AppUser> GetByIdAsync(string userId);
        Task<AppUser> GetByUserNameAsync(string userName);
        Task<bool> IsExistAsync(string userId);
    }
}
