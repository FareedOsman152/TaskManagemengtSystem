using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagmentSystem.Models
{
    public enum UserTaskStatus
    {
        NotStarted,
        InProgress,
        Completed,
        OnHold
    }

    public enum UserTaskPriority
    {
        None,
        Low,
        Medium,
        High
    }

    public enum UserTaskColor
    {
        None,
        Red,
        Blue,
        Green,
        Yellow,
        Gray
    }
   
    public class UserTask
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public bool IsDone { get; set; } = false;
        public UserTaskStatus Status { get; set; } = UserTaskStatus.NotStarted;
        public UserTaskPriority Priority { get; set; } = UserTaskPriority.None;
        public UserTaskColor Color { get; set; } = UserTaskColor.None;

        public DateTime CreatedDate { get; set; }
        public DateTime? LastEditDate { get; set; }
        public DateTime? BeginOn { get; set; }
        public DateTime? EndOn { get; set; }
        public DateTime? RemindMeBeforeBegin { get; set; }
        public DateTime? RemindMeBeforeEnd { get; set; }

        [ForeignKey("Creater")]
        public string? CreaterId { get; set; }
        public AppUser Creater { get; set; } = null!;
        public List<AppUser> Editors { get; set; } = new List<AppUser>();
        public List<TaskEdiotor> TaskEditor { get; set; } = null!;

        [ForeignKey("TaskList")]
        public int TaskListId { get; set; }
        public TaskList TaskList { get; set; } = null!;
        public List<Notification> Notifications { get; set; } = null!;
    }
}
