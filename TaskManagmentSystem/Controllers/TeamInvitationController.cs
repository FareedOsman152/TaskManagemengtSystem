using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using TaskManagmentSystem.Filters;
using TaskManagmentSystem.Models;
using TaskManagmentSystem.Srvices;
using TaskManagmentSystem.Srvices.Interfaces;
using TaskManagmentSystem.ViewModels;

namespace TaskManagmentSystem.Controllers
{
    public class TeamInvitationController : Controller
    {
        private readonly ITeamInvitationService _teamInvitationService;

        public TeamInvitationController(ITeamInvitationService teamInvitationService)
        {
            _teamInvitationService = teamInvitationService;
        }

        private string GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier);
        public IActionResult show()
        {
            return View();
        }

        //[TypeFilter(typeof(TeamPermissionsFilter), Arguments = new object[] { TeamPermissions.SendInvitation })]
        public IActionResult Send(int id)
        {
            var userId = GetUserId();
            var invitationViewModel = new InvitationViewModel { TeamId = id, SenderId = userId };
            return View("Send", invitationViewModel);
        }

        //[TypeFilter(typeof(TeamPermissionsFilter), Arguments = new object[] { TeamPermissions.SendInvitation })]
        public async Task<IActionResult> SaveInvitation(InvitationViewModel invitationViewModel)
        {
            if (!ModelState.IsValid)
                return View("Send", invitationViewModel);

            var userId = GetUserId();
            if (userId != invitationViewModel.SenderId)
                return BadRequest("This user is not the real sender");

            var result = await _teamInvitationService.Send(invitationViewModel);
            if(!result.Succeeded)
                return BadRequest(result.ErrorMessage);

            return RedirectToAction("Show", "Team");
        }

    }
}
