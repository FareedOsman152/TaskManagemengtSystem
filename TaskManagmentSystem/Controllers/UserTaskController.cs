using System.Security.Claims;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using TaskManagmentSystem.Models;
using TaskManagmentSystem.Notifications.Interfaces;
using TaskManagmentSystem.ViewModels;

namespace TaskManagmentSystem.Controllers
{
    public class UserTaskController : Controller
    {    
        private readonly AppDbContext _context;
        private readonly INotificationManager _notificationManager;

        public UserTaskController(AppDbContext context, INotificationManager notificationManager)
        {
            _context = context;
            _notificationManager = notificationManager;
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
            // 1- create the task and add it to DB
            if (ModelState.IsValid)
            {
                var userTask = new UserTask();
                userTask.Title = userTaskFromRequest.Title;
                userTask.Description = userTaskFromRequest.Description;
                userTask.BeginOn = userTaskFromRequest.BeginOn;
                userTask.EndOn = userTaskFromRequest.EndOn;
                userTask.TaskListId = userTaskFromRequest.TaskListId;
                // for test ============
                userTask.BeginOn = DateTime.Now.AddSeconds(20);
                //========
                 _context.UserTasks.Add(userTask);
                 await _context.SaveChangesAsync();

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                await _notificationManager.ManageTaskBeginAndEndAsync(userId!, userTask);

                return RedirectToAction
                    ("ShowAll", "TaskList", new { id = userTaskFromRequest.WorkSpaceId });
            }
            return View("Add", userTaskFromRequest);
        }

        public async Task<IActionResult>Edit(int Id, int WorkSpaceId)
        {
            var userTask = await _context.UserTasks.FindAsync(Id);
            if(userTask is not null)
            {
                var userTaskViewModel = new UserTaskEditViewModel
                {
                    Id = Id,
                    WorkSpaceId = WorkSpaceId,
                    Title = userTask.Title,
                    Description = userTask.Description,
                    Status = userTask.Status,
                    Priority = userTask.Priority,
                    BeginOn = userTask.BeginOn,
                    EndOn = userTask.EndOn
                };
                return View("Edit", userTaskViewModel);
            }
            return RedirectToAction("ShowAll", "TaskList", new { Id = WorkSpaceId });
        }

        public async Task<IActionResult> SaveEdit(UserTaskEditViewModel userTaskFromRequest)
        {
            if(ModelState.IsValid)
            {
                var userTask = await _context.UserTasks.FindAsync(userTaskFromRequest.Id);
                if(userTask is not null)
                {
                    userTask.Title = userTaskFromRequest.Title;
                    userTask.Description = userTaskFromRequest.Description;
                    userTask.Status = userTaskFromRequest.Status;
                    userTask.Priority = userTaskFromRequest.Priority;
                    userTask.BeginOn = userTaskFromRequest.BeginOn;
                    userTask.EndOn = userTaskFromRequest.EndOn;

                    await _context.SaveChangesAsync();
                }
                return RedirectToAction("ShowAll", "TaskList", new { Id = userTaskFromRequest.WorkSpaceId });
            }
            return View("Edit", userTaskFromRequest);
        }

        public async Task<IActionResult> Delete(int id, int workSpaceId)
        {
            var userTask = await _context.UserTasks.FindAsync(id);
            if(userTask is not null)
            {
                _context.UserTasks.Remove(userTask);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("ShowAll", "TaskList", new { Id = workSpaceId });
        }
    }
}
