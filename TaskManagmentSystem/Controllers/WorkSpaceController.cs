using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagmentSystem.Models;
using TaskManagmentSystem.RepoSitories.WorkSpaceRepos;

namespace TaskManagmentSystem.Controllers
{
    public class WorkSpaceController : Controller
    {
        private readonly IWorkSpaceRepository _workSpaceRepository;

        public WorkSpaceController(IWorkSpaceRepository workSpaceRepository)
        {
            _workSpaceRepository = workSpaceRepository;
        }

        [Authorize]
        public async Task<IActionResult> ShowAll()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var workSpaces = await _workSpaceRepository.GetAllWorkSpacesAsync(userId!);
            return View("ShowAll",workSpaces);
        }
    }
}
