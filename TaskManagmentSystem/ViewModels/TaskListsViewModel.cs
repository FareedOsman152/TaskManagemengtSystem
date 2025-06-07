using TaskManagmentSystem.Models;

namespace TaskManagmentSystem.ViewModels
{
    public class TaskListsViewModel
    {
        public int WorkSpaceId{ get; set; }
        public List<TaskListForShowAllViewModel> TaskLists{ get; set; }
    }
}
