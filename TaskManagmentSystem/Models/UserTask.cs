namespace TaskManagmentSystem.Models
{
    public class UserTask
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Discription { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime BeginOn { get; set; }
        public DateTime EndOn { get; set; }
        
        [ForeignKey("TaskList")]
        public int TaskListId { get; set; }    
        public TaskList TaskList { get; set; }    
    }
}
