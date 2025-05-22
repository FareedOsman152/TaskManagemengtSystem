using System.ComponentModel.DataAnnotations;
using TaskManagmentSystem.Models;

namespace TaskManagmentSystem.ViewModels
{
    public class UserTaskAddViewModel
    {
        public int TaskListId { get; set; }
        public int WorkSpaceId { get; set; }
        [Required]
        [MinLength(1)]
        [MaxLength(50)]
        [Display(Name ="Title")]
        public string Title { get; set; }

        [MaxLength(255)]
        [Display(Name = "Description")]
        public string? Description { get; set; }

        public UserTaskStatus Status { get; set; } = UserTaskStatus.NotStarted;
        public UserTaskPriority Priority { get; set; } = UserTaskPriority.None;

        [Display(Name = "Begin on")]
        public DateTime? BeginOn { get; set; }

        [Display(Name = "End on")]
        public DateTime? EndOn { get; set; }
    }
}
