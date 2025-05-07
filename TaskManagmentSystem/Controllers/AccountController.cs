using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TaskManagmentSystem.Models;
using TaskManagmentSystem.ViewModel;

namespace TaskManagmentSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
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
