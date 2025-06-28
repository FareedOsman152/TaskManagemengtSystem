using System.Security.Claims;
using Hangfire;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using TaskManagmentSystem.Models;
using TaskManagmentSystem.Srvices.Interfaces;
using TaskManagmentSystem.ViewModels;

namespace TaskManagmentSystem.Controllers
{
    public class UserTaskController : Controller
    {    
        private readonly AppDbContext _context;
        private readonly INotificationService _notificationService;

        public UserTaskController(AppDbContext context, INotificationService notificationService)
        {
            _context = context;
            _notificationService = notificationService;
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
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var userTask = new UserTask();
                userTask.Title = userTaskFromRequest.Title;
                userTask.Description = userTaskFromRequest.Description;
                userTask.CreaterId = userId; ;
                userTask.BeginOn = userTaskFromRequest.BeginOn;
                if(userTask.BeginOn is not null)
                    userTask.RemindMeBeforeBegin = userTask.BeginOn - userTaskFromRequest.RemindMeBeforeBegin.Minutes();
                userTask.EndOn = userTaskFromRequest.EndOn;
                if(userTask.EndOn is not null)
                    userTask.RemindMeBeforeEnd = userTask.EndOn - userTaskFromRequest.RemindMeBeforeEnd.Minutes();
                userTask.TaskListId = userTaskFromRequest.TaskListId;
                userTask.BeginOn = userTaskFromRequest.BeginOn;
                userTask.EndOn = userTaskFromRequest.EndOn;
                userTask.Color = userTaskFromRequest.Color;
                userTask.Priority = userTaskFromRequest.Priority;
                userTask.Status = userTaskFromRequest.Status; 
                 _context.UserTasks.Add(userTask);
                 await _context.SaveChangesAsync();

                await _notificationService.ManageTaskBeginAndEndAsync(userId!, userTask);

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
                    EndOn = userTask.EndOn,
                    Color = userTask.Color
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
                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    userTask.Title = userTaskFromRequest.Title;
                    userTask.Description = userTaskFromRequest.Description;
                    userTask.LastEditDate = DateTime.Now;
                    userTask.Status = userTaskFromRequest.Status;
                    userTask.Priority = userTaskFromRequest.Priority;
                    userTask.BeginOn = userTaskFromRequest.BeginOn;
                    userTask.EndOn = userTaskFromRequest.EndOn;
                    
                    await _context.SaveChangesAsync();
                    _context.TaskEdiotor.Add(new TaskEdiotor { EditorId = userId, TaskEditedId = userTask.Id });
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

        public async Task<IActionResult> IsDone(CheckIsDoneTaskViewModel taskIsDoneModel)
        {
            if (taskIsDoneModel is not null)
            {
                var userTask = await _context.UserTasks.FindAsync(taskIsDoneModel.Id);
                if (userTask is not null)
                {
                    userTask.IsDone = taskIsDoneModel.IsDone;
                    await _context.SaveChangesAsync();
                    return RedirectToAction("ShowAll", "TaskList", new { Id = taskIsDoneModel.WorkSpaceId});
                }
                return NotFound();
            }
            return BadRequest();
        }
    }
}
