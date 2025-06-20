using System.ComponentModel.DataAnnotations.Schema;
using TaskManagmentSystem.Models;

namespace TaskManagmentSystem.ViewModels
{
    public class TeamsShowViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; } 
        public DateTime DateCreated { get; set; }

        [ForeignKey("Admin")]
        public string AdminId { get; set; } = null!;
        public string AdminName { get; set; } = null!;

        public string UserId { get; set; }
    }
}
