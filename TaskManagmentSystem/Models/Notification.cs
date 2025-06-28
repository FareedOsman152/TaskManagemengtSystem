using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagmentSystem.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public string Details { get; set; } = null!;
        public bool IsRead { get; set; }
        public NotificationType Type { get; set; }
        public DateTime DateToSend { get; set; }

        [ForeignKey("Recipient")]
        public string? RecipientId { get; set; }
        public AppUser? Recipient { get; set; }

        [ForeignKey("Actor")]
        public string? ActorId { get; set; } 
        public AppUser? Actor { get; set; }

        [ForeignKey("UserTask")]
        public int? UserTaskId { get; set; }
        public UserTask? UserTask { get; set; }

        [ForeignKey("TeamInvitation")]
        public int? TeamInvitationId { get; set; }
        public TeamInvitation? TeamInvitation { get; set; }
    }
    public enum NotificationType
    {
        TaskStarted,
        TaskBeforeStarted,
        TaskEnded,
        TaskBeforeEnded,
        TeamInvitationReceived,
        TeamInvitationAccepted,
        TeamInvitationRejected
    }
}
