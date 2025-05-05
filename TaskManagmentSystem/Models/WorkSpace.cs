namespace TaskManagmentSystem.Models
{
    public class WorkSpace
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Discription { get; set; }
        public List<TaskList> TaskList { get; set; }
        
        [ForeignKey("AppUser")]
        public int AppUserId { get; set; }
        public AppUser AppUser { get; set; }
    }
}
