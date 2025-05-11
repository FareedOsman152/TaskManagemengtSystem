using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagmentSystem.Models
{
    public class TaskList
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public List<UserTask> UserTasks { get; set; }
        
        [ForeignKey("WorkSpace")]
        public int WorkSpaceId { get; set; }
        public WorkSpace WorkSpace { get; set; }
    }
}
