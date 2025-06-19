using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagmentSystem.Models
{
    public class TeamAppUser
    {
        [ForeignKey("Team")]
        public int TeamId { get; set; }  
        public Team Team { get; set; } = null!;

        [ForeignKey("User")]
        public string UserId { get; set; } = null!; 
        public AppUser User { get; set; } = null!;
    }
}
