using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagmentSystem.Filters;
using TaskManagmentSystem.Models;
using TaskManagmentSystem.Srvices.Interfaces;
using TaskManagmentSystem.ViewModels;

namespace TaskManagmentSystem.Controllers
{
    [Authorize]
    public class TeamController : Controller
    {
        private readonly ITeamService _teamService;

        public TeamController(ITeamService teamService)
        {
            _teamService = teamService;
        }
        private string GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier);

        public async Task<IActionResult> Show()
        {
            var userId = GetUserId();
            if (userId is null)
                return BadRequest();

            var teamsViewModelResult = await _teamService.GetTeamsForShowAllAsync(userId);
            if (!teamsViewModelResult.Succeeded)
                return BadRequest(teamsViewModelResult.ErrorMessage);

            var teamsViewModel = teamsViewModelResult.Data;
            return View("Show", teamsViewModel);
        }

        public IActionResult Add()
        {
            return View("Add");
        }

        public async Task<IActionResult> SaveAdd(TeamAddViwModel teamFromRequest)
        {
            var userId = GetUserId();
            if (!ModelState.IsValid)
                return RedirectToAction("Add", teamFromRequest);

            var addTeamResult = await _teamService.AddAsync(teamFromRequest, userId!);
            if (!addTeamResult.Succeeded)
                return BadRequest(addTeamResult.ErrorMessage);

            return RedirectToAction("Show");
        }

        [TypeFilter(typeof(TeamPermissionsFilter), Arguments = new object[] { TeamPermissions.Admin })]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = GetUserId();


            var deleteResult = await _teamService.DeleteAsync(id, userId);
            if (!deleteResult.Succeeded)
                return BadRequest(deleteResult.ErrorMessage);

            return RedirectToAction("Show");
        }

        [TypeFilter(typeof(TeamPermissionsFilter), Arguments =new object[] {TeamPermissions.EditTeamDetails})]
        public async Task<IActionResult> Edit(int id)
        {
            var teamResult = await _teamService.GetByIdAsync(id);
            if (!teamResult.Succeeded)
                return BadRequest(teamResult.ErrorMessage);

            var team = teamResult.Data;

            var userId = GetUserId();
            if (userId == team.AdminId)
            {
                var teamViewModel = new TeamEditViewModel
                {
                    Id = team.Id,
                    Title = team.Title,
                    Description = team.Description
                };
                return View("Edit", teamViewModel);
            }
            else return BadRequest();
        }

        [TypeFilter(typeof(TeamPermissionsFilter), Arguments = new object[] { TeamPermissions.AddEditTask })]
        public async Task<IActionResult> SaveEdit(TeamEditViewModel teamFromRequest)
        {
            if (!ModelState.IsValid)
                return View("Edit", teamFromRequest);

            var userId = GetUserId();
            var editResult = await _teamService.EditAsync(teamFromRequest, userId!);
            if (!editResult.Succeeded)
                return BadRequest(editResult.ErrorMessage);

            return RedirectToAction("Show");
        }

        public async Task<IActionResult> Details(int id)
        {
            var userId = GetUserId();
            var teamDetailsResult = await _teamService.GetTeamDetailsInculdeUsersAsync(id, userId);
            if (!teamDetailsResult.Succeeded)
                return BadRequest(teamDetailsResult.ErrorMessage);

            var teamDetails = teamDetailsResult.Data;
            return View("Details", teamDetails);
        }

        
    }
}
