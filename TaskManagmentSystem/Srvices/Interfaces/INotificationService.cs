using TaskManagmentSystem.Helpers;
using TaskManagmentSystem.Models;

namespace TaskManagmentSystem.Srvices.Interfaces
{
    public interface INotificationService
    {
        Task ManageTaskBeginAndEndAsync(string userId, UserTask userTask);
        Task<OperationResult> SendTeamInvitation(TeamInvitation invitation);
        Task<OperationResult> SendTeamInvitationAccepted(TeamInvitation invitation, bool accepted = true);
    }
}
