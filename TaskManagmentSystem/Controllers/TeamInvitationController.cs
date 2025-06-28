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
        public async Task<IActionResult> ShowReceived()
        {
            var userId = GetUserId();
            var invitationsReceivedResult = await _teamInvitationService.GetReceivedForShow(userId);
            if (!invitationsReceivedResult.Succeeded)
                return BadRequest(invitationsReceivedResult.ErrorMessage);
            return View("ShowReceived", invitationsReceivedResult.Data);
        }
        public async Task<IActionResult> ShowSent()
        {
            var userId = GetUserId();
            var invitationsSentResult = await _teamInvitationService.GetSentForShow(userId);
            if (!invitationsSentResult.Succeeded)
                return BadRequest(invitationsSentResult.ErrorMessage);
            return View("ShowSent", invitationsSentResult.Data);
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

            var result = await _teamInvitationService.SendAsync(invitationViewModel);
            if(!result.Succeeded)
                return BadRequest(result.ErrorMessage);

            var invitation = result.Data;

            return RedirectToAction("Show", "Team");
        }

        public async Task<IActionResult> Accept(int id)
        {
            var userId = GetUserId();
            var result = await _teamInvitationService.AcceptAsync(id, userId);
            if (!result.Succeeded)
                return BadRequest(result.ErrorMessage);
            return RedirectToAction("ShowReceived");
        }

        public async Task<IActionResult> Reject(int id)
        {
            var userId = GetUserId();
            var result = await _teamInvitationService.RejectAsync(id, userId);
            if (!result.Succeeded)
                return BadRequest(result.ErrorMessage);
            return RedirectToAction("ShowReceived");
        }

        public async Task<IActionResult> Cancel(int id)
        {
            var result = await _teamInvitationService.CancelAsync(id);
            if (!result.Succeeded)
                return BadRequest(result.ErrorMessage);
            return RedirectToAction("ShowSent");
        }
    }
}
