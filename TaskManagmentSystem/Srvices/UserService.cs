using Microsoft.EntityFrameworkCore;
using TaskManagmentSystem.Helpers;
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
        public async Task<OperationResult<AppUser>> GetByIdAsync(string userId)
        {
            if(string.IsNullOrEmpty(userId))
                return OperationResult<AppUser>.Failure("User ID is null or empty");

            var userResult = await _userRepository.GetByIdAsync(userId);
            if (!userResult.Succeeded)
                return OperationResult<AppUser>.Failure("User not found");

            return OperationResult<AppUser>.Success(userResult.Data);
        }
        public async Task<OperationResult<AppUser>> GetIncludeTeamsAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return OperationResult<AppUser>.Failure("User ID is null or empty");
            var userResult = await _userRepository.GetByIdIncludeTeamsAsync(userId);
            if (!userResult.Succeeded)
                return OperationResult<AppUser>.Failure("User not found");
            return OperationResult<AppUser>.Success(userResult.Data);
        }

        public async Task<OperationResult<AppUser>> GetByUserNameAsync(string userName)
        {
            if(string.IsNullOrEmpty(userName))
                return OperationResult<AppUser>.Failure("User name is null or empty");
            var userResult = await _userRepository.GetByUserNameAsync(userName);
            if (!userResult.Succeeded)
                return OperationResult<AppUser>.Failure("User not found");
            return OperationResult<AppUser>.Success(userResult.Data);
        }

        public async Task<OperationResult> IsExistAsync(string userId)
        {
            return await _userRepository.IsExistAsync(userId);
        }

        public async Task<OperationResult<UserDetailsForTeamViewModel>> GetUserDetailsForTeamDetailsAsync(string userId, string adminId)
        {
            var userResult = await GetByIdAsync(userId);
            if (!userResult.Succeeded)
                return OperationResult<UserDetailsForTeamViewModel>.Failure(userResult.ErrorMessage);

            if (string.IsNullOrEmpty(adminId))
                return OperationResult<UserDetailsForTeamViewModel>.Failure("Admin ID is null or empty");

            var user  = userResult.Data;

            return OperationResult<UserDetailsForTeamViewModel>
                .Success(new UserDetailsForTeamViewModel
                {
                    Id = user.Id,
                    UserName = user.UserName!,
                    IsAdmin = adminId == user.Id,
                });
        }
    }
}
