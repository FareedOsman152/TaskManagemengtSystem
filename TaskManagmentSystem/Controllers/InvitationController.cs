using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using TaskManagmentSystem.Filters;
using TaskManagmentSystem.Models;
using TaskManagmentSystem.ViewModels;

namespace TaskManagmentSystem.Controllers
{
    public class TeamInvitationController : Controller
    {
        private string GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier);
        public IActionResult show()
        {
            return View();
        }

        [TypeFilter(typeof(TeamPermissionsFilter), Arguments = new object[] { TeamPermissions.SendInvitation })]
        public IActionResult Send(int id)
        {
            var userId = GetUserId();
            var invitationViewModel = new InvitationViewModel { TeamId = id, SenderId = userId };
            return View("SendInvitationToAddMember", invitationViewModel);
        }

        [TypeFilter(typeof(TeamPermissionsFilter), Arguments = new object[] { TeamPermissions.SendInvitation })]
        public async Task<IActionResult> SaveInvitation(InvitationViewModel invitationViewModel)
        {

        }

    }
}
