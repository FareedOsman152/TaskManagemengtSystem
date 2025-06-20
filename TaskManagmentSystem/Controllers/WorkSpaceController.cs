using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using TaskManagmentSystem.Models;
using TaskManagmentSystem.ViewModels;

namespace TaskManagmentSystem.Controllers
{
    [Authorize]
    public class WorkSpaceController : Controller
    {
        private readonly AppDbContext _context;

        public WorkSpaceController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> ShowAll()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var workSpaces = _context.WorkSpaces.Where(x => x.AppUserId == userId).ToList();
            return View("ShowAll", workSpaces);
        }

        public async Task<IActionResult> ShowForTeam(int id)
        {
            var team = await _context.Teams.Include(t=>t.Users).FirstOrDefaultAsync(t=>t.Id==id);
            if (team is null)
                return NotFound();

            var workspaces = await _context.WorkSpaces.Where(w=>w.TeamId==id).ToListAsync();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (team.Users.FirstOrDefault(u => u.Id == userId) is null || team.Users.Count==0)
                return BadRequest();

            var workspacesViewModel = new List<WorkSpaceForTeamViewModel>();
            var admin = await _context.Users.FindAsync(team.AdminId);
            if (admin is null)
                return BadRequest();

            var usersViewModel = new List<UserDetailsForTeamViewModel>();

            foreach (var u in team.Users)
            {
                usersViewModel.Add(new UserDetailsForTeamViewModel
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    IsAdmin = u.Id == admin.Id
                });
            }

            var fullWorkspacesViewModel = new FullDataWorkSpaceForTeamViewModel
            {
                Workspaces = workspacesViewModel,
                Name = team.Title,
                Description = team.Description,
                DateCreated = team.DateCreated,
                AdminId = team.AdminId,
                AdminName = admin.UserName,
                UserId = userId
            };
            if (workspaces.Count < 1)
                goto finalReutn;

            foreach (var w in workspaces)
            {
                workspacesViewModel.Add(new WorkSpaceForTeamViewModel
                {
                    Title = w.Title,
                    Description = w.Description,
                    Id = w.Id,
                    Color = w.Color
                });
            }

        finalReutn:
            return View("ShowForTeam", fullWorkspacesViewModel);
        }

        public IActionResult Add()
        {
            var colors = Enum.GetNames(typeof(WorkSpaceColor)).ToList();
            var workSpaceViewModel = new WorkSpaceViewModel
            {                
                Colors = colors
            };
            return View("Add", workSpaceViewModel);
        }

        public async Task<IActionResult> SaveAdd(WorkSpaceViewModel workSpaceFromRequest)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                var workSapce = new WorkSpace();
                workSapce.Title = workSpaceFromRequest.Title;
                workSapce.Description = workSpaceFromRequest.Description;
                workSapce.AppUserId = userId!;
                workSapce.Color = workSpaceFromRequest.Color;

                await _context.AddAsync(workSapce);
                await _context.SaveChangesAsync();
                return RedirectToAction("ShowAll");
            }
            return View("Add", workSpaceFromRequest);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var workSpace = await _context.WorkSpaces.FindAsync(id);
            if (workSpace != null)
            {
                _context.WorkSpaces.Remove(workSpace);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("ShowAll");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var workSpace = await _context.WorkSpaces.FindAsync(id);
            if (workSpace is null)
            {
                return NotFound();
            }
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
                var workSpace = await _context.WorkSpaces.FindAsync(id);
                if (workSpace != null)
                {
                    workSpace.Title = workSpaceFromRequest.Title;
                    workSpace.Description = workSpaceFromRequest.Description;
                    workSpace.Color = workSpaceFromRequest.Color;
                    _context.Update(workSpace);
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction("ShowAll");
            }
            return View("Edit", workSpaceFromRequest);
        }
    }
}
