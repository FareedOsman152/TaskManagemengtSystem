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
                    return RedirectToAction("Index", "Home");
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
    }
}
