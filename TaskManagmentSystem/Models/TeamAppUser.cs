using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagmentSystem.Models
{
    [Flags]
    public enum Permissions
    {
        None = 0,
        AddEditWorkspace = 1,
        AddEditTaskList = 2,
        AddEditTask = 4,
        SendInvitation = 8,
    }

    public class TeamAppUser
    {
        [ForeignKey("Team")]
        public int TeamId { get; set; }  
        public Team Team { get; set; } = null!;

        [ForeignKey("User")]
        public string UserId { get; set; } = null!; 
        public AppUser User { get; set; } = null!;
        public Permissions Permissons { get; set; } = Permissions.None;
    }
}
