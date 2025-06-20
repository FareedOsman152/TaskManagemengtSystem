using TaskManagmentSystem.Models;
using TaskManagmentSystem.Repositories.Interfaces;
using TaskManagmentSystem.Srvices.Interfaces;
using TaskManagmentSystem.ViewModels;

namespace TaskManagmentSystem.Srvices
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<AppUser> GetByIdAsync(string userId)
        {
            Check.IsNull(userId);
            return await _userRepository.GetByIdAsync(userId);
        }
        public async Task<bool> IsExistAsync(string userId)
        {
            return await _userRepository.IsExistAsync(userId); 
        }

        public async Task<UserDetailsForTeamViewModel> GetUserDetailsForTeamDetails(string userId,string adminId)
        {
            var user = await GetByIdAsync(userId);
            return new UserDetailsForTeamViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                IsAdmin = adminId == user.Id,
            };
        }
    }
}
