using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagmentSystem.Models
{
    public enum WorkSpaceColor
    {
        None,
        Red,
        Blue,
        Green,
        Yellow,
        Gray
    }
    public class WorkSpace
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public WorkSpaceColor Color { get; set; } = WorkSpaceColor.None;
        public List<TaskList>? TaskList { get; set; }

        [ForeignKey("AppUser")]
        public string AppUserId { get; set; } = null!;
        public AppUser AppUser { get; set; } = null!;

        [ForeignKey("Team")]
        public int? TeamId { get; set; }
        public Team? Team { get; set; }
    }
}
