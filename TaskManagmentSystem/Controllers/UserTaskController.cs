using Microsoft.AspNetCore.Mvc;
using TaskManagmentSystem.Models;
using TaskManagmentSystem.ViewModels;

namespace TaskManagmentSystem.Controllers
{
    public class UserTaskController : Controller
    {    
        private readonly AppDbContext _context;

        public UserTaskController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult Add(int TaskListId, int WorkSpaceId)
        {
            var userTaskVM = new UserTaskAddViewModel
            {
                TaskListId = TaskListId,
                WorkSpaceId = WorkSpaceId
            };
            return View("Add",userTaskVM);
        }

        public async Task<IActionResult> SaveAdd(UserTaskAddViewModel userTaskFromRequest)
        {
            if (ModelState.IsValid)
            {
                var userTask = new UserTask();
                userTask.Title = userTaskFromRequest.Title;
                userTask.Description = userTaskFromRequest.Description;
                userTask.BeginOn = userTaskFromRequest.BeginOn;
                userTask.EndOn = userTaskFromRequest.EndOn;
                userTask.TaskListId = userTaskFromRequest.TaskListId;
                userTask.CreatedDate = DateTime.Now;

                 _context.UserTasks.Add(userTask);
                 await _context.SaveChangesAsync();

                return RedirectToAction
                    ("ShowAll", "TaskList", new { id = userTaskFromRequest.WorkSpaceId });
            }
            return View("Add", userTaskFromRequest);
        }
    }
}
