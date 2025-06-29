using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagmentSystem.Models
{
    public class Team
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; } 
        public DateTime DateCreated { get; set; }

        [ForeignKey("Admin")]
        public string? AdminId { get; set; } = null!;
        public AppUser? Admin { get; set; } = null!;

        public List<TeamAppUser> TeamAppUsers { get; set; } = null!;

        // with skip
        public List<AppUser> Users { get; set; } = null!;
        public List<WorkSpace> WorkSpaces { get; set; } = null!;
        public List<TeamInvitation> Invitations { get; set; } = null!;
    }
}
