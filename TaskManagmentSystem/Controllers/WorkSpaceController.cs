using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Host;
using Microsoft.EntityFrameworkCore;
using TaskManagmentSystem.Models;
using TaskManagmentSystem.Srvices.Interfaces;
using TaskManagmentSystem.ViewModels;

namespace TaskManagmentSystem.Controllers
{
    [Authorize]
    public class WorkSpaceController : Controller
    {
        private readonly IWorkSpaceService _workSpaceService;
        private readonly ITeamService _teamService;
        private readonly IUserService _userService;

        public WorkSpaceController(IWorkSpaceService workSpaceService, ITeamService teamService, IUserService userService)
        {
            _workSpaceService = workSpaceService;
            _teamService = teamService;
            _userService = userService;
        }

        private string GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier);
        public async Task<IActionResult> ShowAll()
        {
            var userId = GetUserId();
            var workSpacesResult = await _workSpaceService.GetForUserAsync(userId);
            if (!workSpacesResult.Succeeded)
                return BadRequest(workSpacesResult.ErrorMessage);

            return View("ShowAll", workSpacesResult.Data);
        }

        public async Task<IActionResult> ShowForTeam(int id)
        {
            var userId = GetUserId();
            var workSpacesResult = await _workSpaceService.GetForTeamShowAsync(id,userId);
            if (!workSpacesResult.Succeeded)
                return BadRequest(workSpacesResult.ErrorMessage);

            return View("ShowForTeam", workSpacesResult.Data);
        }

        public IActionResult Add(int teamId)
        {
            var colors = Enum.GetNames(typeof(WorkSpaceColor)).ToList();
            var workSpaceViewModel = new WorkSpaceViewModel
            {                
                Colors = colors,
                TeamId = teamId
            };
            return View("Add", workSpaceViewModel);
        }

        public async Task<IActionResult> SaveAdd(WorkSpaceViewModel workSpaceFromRequest)
        {
            var createResult = await _workSpaceService.CreateAsync(workSpaceFromRequest, GetUserId());
            if (!createResult.Succeeded)
            {
                ModelState.AddModelError("", createResult.ErrorMessage);
                return View("Add", workSpaceFromRequest);
            }
            if(workSpaceFromRequest.TeamId == 0)
                return RedirectToAction("ShowAll");
            
            return RedirectToAction("ShowForTeam",new {Id = workSpaceFromRequest.TeamId});
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var deleteResult = await _workSpaceService.DeleteAsync(id);
            if (!deleteResult.Succeeded)
                ModelState.AddModelError("", deleteResult.ErrorMessage);
            return RedirectToAction("ShowAll");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var workSpaceResult = await _workSpaceService.GetByIdAsync(id);
           if (!workSpaceResult.Succeeded)
                return NotFound(workSpaceResult.ErrorMessage);
            var workSpace = workSpaceResult.Data;
            var colors = Enum.GetNames(typeof(WorkSpaceColor)).ToList();
            var workSpaceViewModel = new WorkSpaceForEditViewModel
            {
                Id = workSpace.Id,
                Title = workSpace.Title,
                Description = workSpace.Description,
                Color = workSpace.Color,
                Colors = colors
                
            };
            return View("Edit", workSpaceViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> SaveEdit(int id, WorkSpaceForEditViewModel workSpaceFromRequest)
        {
            if (ModelState.IsValid)
            {
               var updateResult = await _workSpaceService.UpdateAsync(workSpaceFromRequest);
                if (!updateResult.Succeeded)
                {
                    ModelState.AddModelError("", updateResult.ErrorMessage);
                    return View("Edit", workSpaceFromRequest);
                }
                return RedirectToAction("ShowAll");
            }
            return View("Edit", workSpaceFromRequest);
        }
    }
}
