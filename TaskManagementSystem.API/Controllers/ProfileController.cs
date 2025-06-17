using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagmentSystem.Models;
using TaskManagmentSystem.ViewModels;

namespace TaskManagementSystem.API.Controllers
{
    [Route("api/Profile")]
    //[Authorize]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProfileController(AppDbContext context)
        {
            _context = context;
        }

        //[HttpGet("${id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetProfileData(string id)
        {
            //var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var profile = _context.AppUserProfiles.FirstOrDefault(p => p.AppUserId == id);
            if (profile is null)
            {
                return NotFound();
            }
            var profileResponse = new ProfileShowViewModel
            {
                Id = profile.Id,
                FirstName = profile.FirstName,
                LastName = profile.LastName,
                PicURL = profile.PicURL,
                JopTitle = profile.JopTitle
            };
            return Ok(profileResponse);
        }
    }
}
