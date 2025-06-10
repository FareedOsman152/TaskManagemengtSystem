using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagmentSystem.Models;
using TaskManagmentSystem.ViewModels;

namespace TaskManagmentSystem.Controllers
{
    [Authorize]
    public class TaskListController : Controller
    {
        private readonly AppDbContext _context;

        public TaskListController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult ShowAll(int id)
        {
            var taskLists = new TaskListsViewModel();
            taskLists.TaskLists = _context.TaskLists
                .Where(x => x.WorkSpaceId == id)
                .Select(x => new TaskListForShowAllViewModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    Description = x.Description,
                    Color = x.color,
                    UserTasks = x.UserTasks.Select(x => new UserTaskForShowListsViewModel
                    {
                        Id = x.Id,
                        Title = x.Title,
                        Description = x.Description,
                        Status = x.Status,
                        Priority = x.Priority,
                        Color = x.Color,
                        CreatedDate = x.CreatedDate,
                        BeginOn = x.BeginOn,
                        EndOn = x.EndOn,
                        IsDone = x.IsDone
                    }).ToList()
                })
                .ToList();
            taskLists.WorkSpaceId = id;

            return View("ShowAll",taskLists);
        }

        [HttpGet]
        public IActionResult Add(int id)
        {
            var taskListModel = new TaskListWithWorkSpaceIdViewModel();
            taskListModel.WorkSpaceId = id;
            return View("Add",taskListModel);
        }

        [HttpPost]
        public async Task<IActionResult> SaveAdd(TaskListWithWorkSpaceIdViewModel taskListFromRequst)
        {
            if (ModelState.IsValid)
            {
                var taskList = new TaskList();
                taskList.Title = taskListFromRequst.Tilte;
                taskList.Description = taskListFromRequst.Description;
                taskList.WorkSpaceId = taskListFromRequst.WorkSpaceId;
                _context.TaskLists.Add(taskList);
                await _context.SaveChangesAsync();
                return RedirectToAction("ShowAll",new {id = taskList.WorkSpaceId});
            }
            return View("Add", taskListFromRequst);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int taskListId, int workSpaceId)
        {
            var taskList = await _context.TaskLists.FindAsync(taskListId);
            if (taskList is not null)
            {
                _context.TaskLists.Remove(taskList);
                await _context.SaveChangesAsync();
                return RedirectToAction("ShowAll", new { id = taskList.WorkSpaceId });
            }
            return RedirectToAction("ShowAll", new { id = workSpaceId });
        }

        public async Task<IActionResult> Edit(int id)
        {
            var taskList = await _context.TaskLists.FindAsync(id);
            if (taskList is not null)
            {
                var taskListViewModel = new TaskListForEditViewModel
                {
                    Id = taskList.Id,
                    WorkSpaceId = taskList.WorkSpaceId,
                    Tilte = taskList.Title,
                    Description = taskList.Description
                };
                return View("Edit", taskListViewModel);
            }
            return View("Edit");
        }

        public async Task<IActionResult> SaveEdit(TaskListForEditViewModel taskListFromRequest)
        {
            if(ModelState.IsValid)
            {
                var TaskList = await _context.TaskLists.FindAsync(taskListFromRequest.Id);
                if (TaskList is not null)
                {
                    TaskList.Title = taskListFromRequest.Tilte;
                    TaskList.Description = taskListFromRequest.Description;
                    await _context.SaveChangesAsync();
                    return RedirectToAction("ShowAll", new { Id = taskListFromRequest.WorkSpaceId });
                }
            }

            return View("Edit", taskListFromRequest);
        }
    }
}
