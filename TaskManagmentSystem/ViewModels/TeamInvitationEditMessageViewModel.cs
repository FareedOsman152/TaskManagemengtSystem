using TaskManagmentSystem.Models;

namespace TaskManagmentSystem.ViewModels
{
    public class TeamInvitationEditMessageViewModel
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public TeamPermissions Permissions{ get; set; } = TeamPermissions.None;
    }
}
