using TaskManagmentSystem.Models;

namespace TaskManagmentSystem.ViewModels
{
    public class TaskListForShowAllViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public UserTaskListColor Color { get; set; } = UserTaskListColor.None;
        public List<UserTaskForShowListsViewModel> UserTasks { get; set; }
    }
}
