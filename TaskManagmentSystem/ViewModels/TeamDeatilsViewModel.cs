using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagmentSystem.ViewModels
{
    public class TeamDeatilsViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime DateCreated { get; set; }

        [ForeignKey("Admin")]
        public string AdminId { get; set; } = null!;
        public List<UserDetailsForTeamViewModel> Users { get; set; }

        public string UserId { get; set; }
    }
}
