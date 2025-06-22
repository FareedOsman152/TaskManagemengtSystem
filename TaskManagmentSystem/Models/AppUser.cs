using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace TaskManagmentSystem.Models
{
    public enum Genders
    {
        Male,
        Female
    }
    public class AppUser : IdentityUser
    {
        public DateOnly Birthday{ get; set; }
        public Genders Gender{ get; set; }
        public AppUserProfile Profile { get; set; } = null!;
        public List<WorkSpace>? WorkSpaces { get; set; }
        public List<Notification>? Notifications { get; set; }

        // M:N
        public List<TeamAppUser> TeamAppUsers { get; set; } = null!;

        // with skip
        public List<Team>? Teams { get; set; } = new List<Team>();
        public List<UserTask> TasksCreated{ get; set; } = new List<UserTask>();

        public List<UserTask> TasksEdited { get; set; } = new List<UserTask>();
        public List<TaskEdiotor> TaskEditor { get; set; } = null!;

        public List<TeamInvitation> TnvitationsSent { get; set; } = new List<TeamInvitation>();
        public List<TeamInvitation> TnvitationsReceived { get; set; } = new List<TeamInvitation>();
    }
}
