using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagmentSystem.Models;
using TaskManagmentSystem.ViewModels;

namespace TaskManagmentSystem.Controllers
{
    public class TaskListController : Controller
    {
        private readonly AppDbContext _context;

        public TaskListController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult ShowAll(int id)
        {
            var taskLists = new TaskListsViewModel();
            taskLists.TaskLists = _context.TaskLists
                .Where(x => x.WorkSpaceId == id)
                .Include(x=>x.UserTasks)
                .ToList();

            return View("ShowAll",taskLists);
        }
    }
}
