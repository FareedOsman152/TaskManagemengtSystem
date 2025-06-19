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
                        AdminName = admin!.UserName!,
                        UserId = userId!
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

        public async Task<IActionResult> Delete(int id)
        {
            var team = await _context.Teams.FirstOrDefaultAsync(t => t.Id == id);
            if (team == null)
                return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == team.AdminId)
            {
                _context.Teams.Remove(team);
                await _context.SaveChangesAsync();
                return RedirectToAction("Show");
            }
            else return BadRequest();
        }

        public async Task<IActionResult> Edit(int id)
        {
            var team = await _context.Teams.FirstOrDefaultAsync(t => t.Id == id);
            if (team == null)
                return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
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

        public async Task<IActionResult> SaveEdit(TeamEditViewModel teamFromRequest)
        {
            if (!ModelState.IsValid)
                return View("Edit", teamFromRequest);

            var team = await _context.Teams.FirstOrDefaultAsync(t => t.Id == teamFromRequest.Id);
            if (team is null)
                return NotFound();

            if (team.AdminId != User.FindFirstValue(ClaimTypes.NameIdentifier))
                return BadRequest();

            team.Title = teamFromRequest.Title;
            team.Description = teamFromRequest.Description;

            await _context.SaveChangesAsync();
            return RedirectToAction("Show");
        }

        public async Task<IActionResult> Details(int id)
        {
            var team = await _context.Teams.Include(t=>t.Users).FirstOrDefaultAsync(t => t.Id == id);
            if (team == null)
                return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (team.Users.FirstOrDefault(u => u.Id == userId) is null)
                return BadRequest();

            var usersDeatils = new List<UserDetailsForTeamViewModel>();
            foreach (var user in team.Users)
            {
                usersDeatils.Add(new UserDetailsForTeamViewModel
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    IsAdmin = team.AdminId==user.Id,
                });
            }


            var teamDetails = new TeamDeatilsViewModel
            {
                Id = team.Id,
                Title = team.Title,
                Description = team.Description,
                AdminId = team.AdminId,
                UserId = userId,
                Users = usersDeatils
            };

            return View("Details", teamDetails);
        }
    }
}
