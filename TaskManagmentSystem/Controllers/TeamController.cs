using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagmentSystem.Models;
using TaskManagmentSystem.ViewModels;

namespace TaskManagmentSystem.Controllers
{
    [Authorize]
    public class TeamController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public TeamController(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Show()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userWithTeams = await _context.Users.Include(u => u.Teams)
                .FirstOrDefaultAsync(u => u.Id == userId);
            if (userWithTeams is null)
            {
                return NotFound();
            }

            var teamsViewModel = new List<TeamsShowViewModel>();
            
            if(userWithTeams.Teams is not null)
            {
                foreach (var t in userWithTeams.Teams)
                {
                    var admin = await _context.Users.FirstOrDefaultAsync(u => u.Id == t.AdminId);
                    teamsViewModel.Add(new TeamsShowViewModel
                    {
                        Id = t.Id,
                        Name = t.Title,
                        Description = t.Description,
                        DateCreated = t.DateCreated,
                        AdminId = t.AdminId,
                        AdminName = admin!.UserName!
                    });
                }
            }  

            return View("Show", teamsViewModel);
        }

        public IActionResult Add()
        {
            return View("Add");
        }

        public async Task<IActionResult> SaveAdd(TeamAddViwModel teamFromRequest)
        {
            var user = await _userManager.GetUserAsync(User);
            if (!ModelState.IsValid)
                return RedirectToAction("Add", teamFromRequest);

            var team = new Team
            {
                Title = teamFromRequest.Title,
                Description = teamFromRequest.Description,
                DateCreated = DateTime.Now,
                AdminId = user!.Id
            };
            await _context.AddAsync(team);

            await _context.SaveChangesAsync();
           // id of team
            await _context.TeamAppUser.AddAsync(new TeamAppUser { TeamId = team.Id, UserId = user.Id });
            await _context.SaveChangesAsync();

            return RedirectToAction("Show");
        }
    }
}
