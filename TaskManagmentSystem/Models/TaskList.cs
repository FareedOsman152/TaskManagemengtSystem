namespace TaskManagmentSystem.Models
{
    public class TaskList
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Discription { get; set; }
        public List<UserTask> MyProperty { get; set; }
    }
}
