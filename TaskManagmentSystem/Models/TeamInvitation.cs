namespace TaskManagmentSystem.Models
{
    public enum InvitationStatus
    {
        Pending = 0,
        Accepted = 1,
        Rejected = 2
    }
    public class TeamInvitation
    {
        public int Id { get; set; }
        public string SenderId { get; set; } = null!;
        public AppUser Sender { get; set; } = null!;
        public string ReceiverId { get; set; } = null!;
        public AppUser Receiver { get; set; } = null!;
        public int TeamId { get; set; }
        public Team Team { get; set; } = null!;
        public string? Message { get; set; }
        public DateTime SendOn { get; set; }
        public InvitationStatus Status { get; set; } = InvitationStatus.Pending;
        public TeamPermissions Permissions { get; set; } = TeamPermissions.None;
    }
}
