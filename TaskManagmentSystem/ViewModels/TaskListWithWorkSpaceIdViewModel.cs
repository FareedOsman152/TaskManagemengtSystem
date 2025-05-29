using System.ComponentModel.DataAnnotations;
using TaskManagmentSystem.Models;

namespace TaskManagmentSystem.ViewModels
{
    public class TaskListWithWorkSpaceIdViewModel
    {
        public int WorkSpaceId { get; set; }
        [Required]
        [Display(Name = "Tilte")]
        public string Tilte { get; set; } = null!;

        [Display(Name = "Description")]
        public string? Description { get; set; }
        public UserTaskListColor color { get; set; } = UserTaskListColor.None;
    }
}
