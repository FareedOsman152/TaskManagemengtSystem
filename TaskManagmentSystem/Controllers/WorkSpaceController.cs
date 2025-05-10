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
            var workSpaces = _context.WorkSpaces.Where(x => x.AppUserId == userId).ToList();
            return View("ShowAll", workSpaces);
        }

        public IActionResult Add()
        {
            return View("Add");
        }

        public async Task<IActionResult> SaveAdd(WorkSpaceViewModel workSpaceFromRequest)
        {
            if (ModelState.IsValid)
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

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var workSpace = await _context.WorkSpaces.FindAsync(id);
            if (workSpace != null)
            {
                _context.WorkSpaces.Remove(workSpace);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("ShowAll");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var workSpace = await _context.WorkSpaces.FindAsync(id);
            if (workSpace is null)
            {
                return NotFound();
            }
            var workSpaceViewModel = new WorkSpaceForEditViewModel
            {
                Id = workSpace.Id,
                Tilte = workSpace.Title,
                Discription = workSpace.Discription
            };
            return View("Edit", workSpaceViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> SaveEdit(int id, WorkSpaceForEditViewModel workSpaceFromRequest)
        {
            if (ModelState.IsValid)
            {
                var workSpace = await _context.WorkSpaces.FindAsync(id);
                if (workSpace != null)
                {
                    workSpace.Title = workSpaceFromRequest.Tilte;
                    workSpace.Discription = workSpaceFromRequest.Discription;
                    _context.Update(workSpace);
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction("ShowAll");
            }
            return View("Edit", workSpaceFromRequest);
        }
    }
}
