using TaskManagmentSystem.Models;

namespace TaskManagmentSystem.ViewModels
{
    public class UserTaskForShowListsViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public UserTaskStatus Status { get; set; } = UserTaskStatus.NotStarted;
        public UserTaskPriority Priority { get; set; } = UserTaskPriority.None;
        public UserTaskColor Color { get; set; } = UserTaskColor.None;
        public bool IsDone { get; set; } = false;

        public DateTime CreatedDate { get; set; }
        public DateTime? BeginOn { get; set; }
        public DateTime? EndOn { get; set; }
    }
}
