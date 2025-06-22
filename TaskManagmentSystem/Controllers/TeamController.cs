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
        private readonly ITeamAppUserService _teamAppUserService;

        public TeamController(ITeamService teamService, ITeamAppUserService teamAppUserService)
        {
            _teamService = teamService;
            _teamAppUserService = teamAppUserService;
        }

        private string GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier);

        public async Task<IActionResult> Show()
        {
            var userId = GetUserId();
            if (userId is null)
                return BadRequest();
             
            var teamsViewModel = await _teamService.GetTeamsForShowAllAsync(userId);

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

            var team = await _teamService.AddAsync(teamFromRequest, userId!);
            if (team == null) 
                return BadRequest("Failed to create team");

            await _teamAppUserService.AddAsync(userId!, team.Id);

            return RedirectToAction("Show");
        }

        [TypeFilter(typeof(TeamPermissionsFilter), Arguments = new object[] { TeamPermissions.Admin })]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = GetUserId();


            await _teamService.DeleteAsync(id, userId);
            return RedirectToAction("Show");
        }

        [TypeFilter(typeof(TeamPermissionsFilter), Arguments =new object[] {TeamPermissions.EditTeamDetails})]
        public async Task<IActionResult> Edit(int id)
        {
            var team = await _teamService.GetByIdAsync(id);
            if (team == null)
                return NotFound();

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
            await _teamService.EditAsync(teamFromRequest, userId!);
            return RedirectToAction("Show");
        }

        public async Task<IActionResult> Details(int id)
        {
            var userId = GetUserId();
            var teamDetails = await _teamService.GetTeamDetailsInculdeUsersAsync(id, userId);
            return View("Details", teamDetails);
        }

        
    }
}
