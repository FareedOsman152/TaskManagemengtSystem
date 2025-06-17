using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TaskManagmentSystem.Models;
using TaskManagmentSystem.ViewModels;

namespace TaskManagmentSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly AppDbContext _context;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, AppDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View("Register");
        }

        [HttpPost]
        public async Task<IActionResult> SaveRegister(UserRegisterViewModel userFromRequest)
        {
            if(ModelState.IsValid)
            {
                var user = new AppUser();
                user.UserName = userFromRequest.Username;
                user.Email = userFromRequest.Email;
                IdentityResult saveResult =
                    await _userManager.CreateAsync(user,userFromRequest.Password);
                if(saveResult.Succeeded)
                {
                    var userId = user.Id;
                    var profile = new AppUserProfile
                    {
                        FirstName = userFromRequest.FirstName,
                        LastName = userFromRequest.LastName,
                        JopTitle = userFromRequest.JopTitle,
                        AppUserId = userId
                    };
                    _context.AppUserProfiles.Add(profile);
                    await _context.SaveChangesAsync();
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("ShowAll", "WorkSpace");
                }
                else
                {
                    foreach (var error in saveResult.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View("Register",userFromRequest);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View("Login");
        }

        [HttpPost]
        public async Task<IActionResult> SaveLogin(UserForLogin userFromRequest)
        {
            if (ModelState.IsValid)
            {
                AppUser? user = await _userManager.FindByNameAsync(userFromRequest.Username);
                if (user is not null)
                {
                    var isValidPassword = await _userManager.CheckPasswordAsync(user, userFromRequest.Password);
                    if (isValidPassword)
                    {
                        await _signInManager.SignInAsync(user,userFromRequest.RememberMe);
                        return RedirectToAction("ShowAll", "WorkSpace");
                    }
                }
            }
            return View("Login",userFromRequest);
        }

        public async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }
    }
}
