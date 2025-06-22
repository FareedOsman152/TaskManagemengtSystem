using Microsoft.AspNetCore.Mvc;

namespace TaskManagmentSystem.Controllers
{
    public class InvitationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
