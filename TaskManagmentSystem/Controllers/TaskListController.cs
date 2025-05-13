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
                .Include(x=>x.UserTasks)
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
    }
}
