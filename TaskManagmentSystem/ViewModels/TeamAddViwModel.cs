using System.ComponentModel.DataAnnotations;
using TaskManagmentSystem.Models;

namespace TaskManagmentSystem.ViewModels
{
    public class TeamAddViwModel
    {
        [Required]
        [MinLength(1)]
        [MaxLength(50)]
        public string Title { get; set; } = null!;

        [MinLength(1)]
        [MaxLength(255)]
        public string? Description { get; set; }
        public TeamPermissions Permissions { get; set; } = TeamPermissions.None;
    }
}
