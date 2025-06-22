using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagmentSystem.Models
{
    [Flags]
    public enum TeamPermissions
    {
        None = 0,
        Admin = 1,
        AddEditWorkspace = 2,
        AddEditTaskList = 4,
        AddEditTask = 8,
        SendInvitation = 16,
        EditTeamDetails = 32,
    }

    public class TeamAppUser
    {
        [ForeignKey("Team")]
        public int TeamId { get; set; }  
        public Team Team { get; set; } = null!;

        [ForeignKey("User")]
        public string UserId { get; set; } = null!; 
        public AppUser User { get; set; } = null!;
        public TeamPermissions Permissons { get; set; } = TeamPermissions.None;
    }
}
