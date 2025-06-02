using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagmentSystem.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public String Details { get; set; } = null!;
        public DateTime DateCeated { get; set; } = DateTime.Now;

        [ForeignKey("AppUser")]
        public string? AppUserId { get; set; }
        public AppUser? AppUser { get; set; }

        [ForeignKey("UserTask")]
        public int? UserTaskId { get; set; }
        public UserTask? UserTask { get; set; }
        public bool IsRead { get; set; }
    }
}
