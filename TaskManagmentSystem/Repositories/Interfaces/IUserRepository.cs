using TaskManagmentSystem.Models;

namespace TaskManagmentSystem.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<AppUser> GetByIdAsync(string userId);
        Task<bool> IsExistAsync(string userId);
    }
}
