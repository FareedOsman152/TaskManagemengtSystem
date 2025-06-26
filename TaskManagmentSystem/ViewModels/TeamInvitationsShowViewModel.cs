using TaskManagmentSystem.Models;

namespace TaskManagmentSystem.ViewModels
{
    public class TeamInvitationsShowViewModel
    {
        public int Id { get; set; }
        public string Message { get; set; } = null!;
        public string OtherUserName { get; set; } = null!;
        public string TeamName { get; set; } = null!;
        public InvitationStatus Status { get; set; } = InvitationStatus.Pending;
        public DateTime SendOn { get; set; } = DateTime.Now;
    }
}
