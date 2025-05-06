using Microsoft.AspNetCore.Mvc;

namespace TaskManagmentSystem.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Register()
        {
            return View("Register");
        }

        [HttpPost]
        public IActionResult SaveRegister()
        {
            //logic
            return View("Register");
        }
    }
}
