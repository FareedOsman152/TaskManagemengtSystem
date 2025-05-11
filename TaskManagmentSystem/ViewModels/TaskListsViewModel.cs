using TaskManagmentSystem.Models;

namespace TaskManagmentSystem.ViewModels
{
    public class TaskListsViewModel
    {
        public int WorkSpaceId{ get; set; }
        public List<TaskList> TaskLists{ get; set; }
    }
}
