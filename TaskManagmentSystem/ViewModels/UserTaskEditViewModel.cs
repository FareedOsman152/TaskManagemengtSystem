using System.ComponentModel.DataAnnotations;
using TaskManagmentSystem.Models;

namespace TaskManagmentSystem.ViewModels
{
    public class UserTaskEditViewModel
    {
        public int Id { get; set; }
        public int WorkSpaceId { get; set; }
        [Required]
        [MinLength(1)]
        [MaxLength(50)]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [MaxLength(255)]
        [Display(Name = "Description")]
        public string? Description { get; set; }

        public UserTaskStatus Status { get; set; } = UserTaskStatus.NotStarted;
        public UserTaskPriority Priority { get; set; } = UserTaskPriority.None;
        public UserTaskColor Color { get; set; } = UserTaskColor.None;

        [Display(Name = "Begin on")]
        public DateTime? BeginOn { get; set; }

        [Display(Name = "End on")]
        public DateTime? EndOn { get; set; }

        [Display(Name = "Remind me before begin")]
        [Range(5, 60)]
        public int RemindMeBeforeBegin { get; set; }

        [Display(Name = "Remind me before End")]
        [Range(5, 60)]
        public int RemindMeBeforeEnd { get; set; }
    }
}
