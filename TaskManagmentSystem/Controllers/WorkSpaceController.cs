using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagmentSystem.Models;

namespace TaskManagmentSystem.Controllers
{
    public class WorkSpaceController : Controller
    {
        public AppDbContext _context { get; set; }

        public WorkSpaceController(AppDbContext context)
        {
            _context = context;
        }

        [Authorize]
        public IActionResult ShowAll()
        {
            var workSpaces = _context.
            return View();
        }
    }
}
