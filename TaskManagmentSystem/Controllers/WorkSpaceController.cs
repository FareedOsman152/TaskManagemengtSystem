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
    public class WorkSpaceController : Controller
    {
        private readonly AppDbContext _context;

        public WorkSpaceController(AppDbContext context)
        {
            _context = context;
        }

        [Authorize]
        public async Task<IActionResult> ShowAll()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var workSpaces = _context.WorkSpaces.Where(x=>x.AppUserId==userId).ToList();
            return View("ShowAll",workSpaces);
        }

        public IActionResult Add()
        {
            return View("Add");
        }

        public async Task<IActionResult> SaveAdd(WorkSpaceViewModel workSpaceFromRequest)
        {
            if(ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                var workSapce = new WorkSpace();
                workSapce.Title = workSpaceFromRequest.Tilte;
                workSapce.Discription = workSpaceFromRequest.Discription;
                workSapce.AppUserId = userId!;

                await _context.AddAsync(workSapce);
                await _context.SaveChangesAsync();
                return RedirectToAction("ShowAll");
            }
            return View("Add", workSpaceFromRequest);
        }
    }
}
