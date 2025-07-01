using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Host;
using TaskManagmentSystem.Helpers;
using TaskManagmentSystem.Models;
using TaskManagmentSystem.Repositories.Interfaces;
using TaskManagmentSystem.Srvices.Interfaces;
using TaskManagmentSystem.Srvicese;
using TaskManagmentSystem.ViewModels;

namespace TaskManagmentSystem.Srvices
{
    public class WorkSpaceService : IWorkSpaceService
    {
        private readonly IWorkSpaceRepository _workSpaceRepository;
        private readonly ITeamService _teamService;
        private readonly ITeamAppUserService _teamAppUserService;

        public WorkSpaceService(IWorkSpaceRepository workSpaceRepository, ITeamService teamService, ITeamAppUserService teamAppUserService)
        {
            _workSpaceRepository = workSpaceRepository;
            _teamService = teamService;
            _teamAppUserService = teamAppUserService;
        }

        public async Task<OperationResult<WorkSpace>> GetByIdAsync(int id)
        {
            return await _workSpaceRepository.GetByIdAsync(id);
        }
        public async Task<OperationResult> CreateAsync(WorkSpaceViewModel workSpaceToCreate, string userId)
        {
            var workSpace = new WorkSpace
            {
                Title = workSpaceToCreate.Title,
                Description = workSpaceToCreate.Description,
                AppUserId = userId,
                Color = workSpaceToCreate.Color,
                TeamId = workSpaceToCreate.TeamId > 0 ? workSpaceToCreate.TeamId : null
            };
            return await _workSpaceRepository.CreateAsync(workSpace);
        }
        public async Task<OperationResult> UpdateAsync(WorkSpaceForEditViewModel workSpaceToUpdate)
        {
            var workSpaceResult = await _workSpaceRepository.GetByIdAsync(workSpaceToUpdate.Id);
            if (!workSpaceResult.Succeeded)
                return OperationResult.Failure("WorkSpace not found");

            var workSpace = new WorkSpace
            {
                Title = workSpaceToUpdate.Title,
                Description = workSpaceToUpdate.Description,
            };
            return await _workSpaceRepository.UpdateAsync(workSpace);
        }
        public async Task<OperationResult> DeleteAsync(int id)
        {
            return await _workSpaceRepository.DeleteAsync(id);
        }
        public async Task<OperationResult<List<WorkSpace>>> GetForUserAsync(string userId)
        {
            return await _workSpaceRepository.GetForUserAsync(userId);
        }
        public async Task<OperationResult<List<WorkSpace>>> GetForTeamAsync(int teamId, string userId)
        {
            return await _workSpaceRepository.GetForTeamAsync(teamId);
        }

        public async Task<OperationResult<FullDataWorkSpaceForTeamViewModel>> GetForTeamShowAsync(int teamId, string userId)
        {
            // get the team include users
            var teamResult = await _teamService.GetByIdIncludeUsersAsync(teamId);
            if (!teamResult.Succeeded)
                return OperationResult<FullDataWorkSpaceForTeamViewModel>.Failure(teamResult.ErrorMessage);

            var teamWithUsers = teamResult.Data;

            // check if the user is a member in this team
            var isMemberResult = await _teamAppUserService.IsMemberAsync(userId, teamId);
            if (!isMemberResult.Succeeded)
                return OperationResult<FullDataWorkSpaceForTeamViewModel>.Failure(isMemberResult.ErrorMessage);

            if (isMemberResult.Data == false)
                return OperationResult<FullDataWorkSpaceForTeamViewModel>.Failure($"the user with id {userId} is not a member in this team with id {teamId}");

            // get workspaces of the team
            var workSpacesResult = await GetForTeamAsync(teamId, userId);
            if (!workSpacesResult.Succeeded)
                return OperationResult<FullDataWorkSpaceForTeamViewModel>.Failure(workSpacesResult.ErrorMessage);

            // if there is no workspaces return with empty list of workspaces
            var workSpaces = workSpacesResult.Data;
            if (workSpaces.Count <= 0)
                return OperationResult<FullDataWorkSpaceForTeamViewModel>.
                    Success(new FullDataWorkSpaceForTeamViewModel
                    {
                        Workspaces = new List<WorkSpaceForTeamViewModel>(),
                        Name = teamWithUsers.Title,
                        AdminId = teamWithUsers.AdminId,
                        UserId = userId,
                        TeamId = teamId
                    });

            var workspacesViewModel = new List<WorkSpaceForTeamViewModel>();
            foreach (var w in workSpaces)
            {
                workspacesViewModel.Add(new WorkSpaceForTeamViewModel
                {
                    Title = w.Title,
                    Description = w.Description,
                    Id = w.Id,
                    Color = w.Color
                });
            }

            var fullWorkspacesViewModel = new FullDataWorkSpaceForTeamViewModel
            {
                Workspaces = workspacesViewModel,
                Name = teamWithUsers.Title,
                AdminId = teamWithUsers.AdminId,
                UserId = userId,
                TeamId = teamId
            };

            return OperationResult<FullDataWorkSpaceForTeamViewModel>.Success(fullWorkspacesViewModel);
        }
    }
}
