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
    public class ProfileController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public ProfileController(AppDbContext context, UserManager<AppUser> userManager, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _userManager = userManager;
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Show()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return View("Show",userId);
        }

        public async Task<IActionResult> Edit()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound();

            var profile = await _context.AppUserProfiles.FirstOrDefaultAsync(p => p.AppUserId == user.Id);
            if (profile == null)
                return NotFound();

            var profileViewModel = new ProfilrEditViewModel
            {
                Id = profile.Id,
                FirstName = profile.FirstName,
                LastName = profile.LastName,
                JopTitle = profile.JopTitle,
                PicURL = profile.PicURL
            };

            return View("Edit", profileViewModel);
        }

        public async Task<IActionResult> SaveEdit(ProfilrEditViewModel profileFromRequst)
        {
            if(!ModelState.IsValid)
                return RedirectToAction("Edit", profileFromRequst);

            var profile = await _context.AppUserProfiles.FindAsync(profileFromRequst.Id);
            if(profile is not null)
            {
                profile.FirstName = profileFromRequst.FirstName;
                profile.LastName = profileFromRequst.LastName;
                profile.JopTitle = profileFromRequst.JopTitle;
                if(profileFromRequst.ProfileImage != null && profileFromRequst.ProfileImage.Length>0)
                {
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                    var fileExtension = Path.GetExtension(profileFromRequst.ProfileImage.FileName).ToLower();

                    if (!allowedExtensions.Contains(fileExtension))
                    {
                        return BadRequest("enter pic");
                    }

                    var fileName = Guid.NewGuid().ToString() + fileExtension;
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(),
                              "wwwroot/uploads/ProfileImages",
                              fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await profileFromRequst.ProfileImage.CopyToAsync(stream);
                    }

                    profile.PicURL = $"/uploads/ProfileImages/{fileName}"; 
                    
                }
                await _context.SaveChangesAsync();

                return RedirectToAction("Show", new { Id = profileFromRequst.Id });
            }
            return RedirectToAction("Edit", profileFromRequst);

        }
    }
}
