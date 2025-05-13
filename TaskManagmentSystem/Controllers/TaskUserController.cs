using Microsoft.AspNetCore.Mvc;

namespace TaskManagmentSystem.Controllers
{
    public class TaskUserController : Controller
    {
        public IActionResult Add()
        {
            return View("Add");
        }
    }
}
