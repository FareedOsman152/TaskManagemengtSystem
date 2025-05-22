using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagmentSystem.Models
{
    public enum UserTaskListColor
    {
        None,
        Red,
        Blue,
        Green,
        Yellow,
        Gray
    }
    public class TaskList
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public UserTaskListColor color { get; set; } = UserTaskListColor.None;
        public List<UserTask> UserTasks { get; set; }
        
        [ForeignKey("WorkSpace")]
        public int WorkSpaceId { get; set; }
        public WorkSpace WorkSpace { get; set; }
    }
}
