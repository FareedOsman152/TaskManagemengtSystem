using TaskManagmentSystem.Helpers;
using TaskManagmentSystem.Models;
using TaskManagmentSystem.ViewModels;

namespace TaskManagmentSystem.Srvices.Interfaces
{
    public interface ITeamInvitationService
    {
        Task<OperationResult<List<TeamInvitation>>> GetForReceiverAsync(string userId);
        Task<OperationResult<List<TeamInvitation>>> GetForSenderAsync(string userId);
        Task<bool> IsInvited(string userName, int teamId);
        Task<OperationResult> Send(InvitationViewModel invitationToSend);
        Task<OperationResult> ChangeMessageAsync(TeamInvitationEditMessageViewModel invitationToUpdate);
        Task<OperationResult> AcceptAsync(int id, string userId);
        Task<OperationResult> RejectAsync(int id, string userId);
        Task<OperationResult> CancelAsync(int id);
    }
}
