using TaskManagmentSystem.Helpers;
using TaskManagmentSystem.Models;
using TaskManagmentSystem.ViewModels;

namespace TaskManagmentSystem.Srvices.Interfaces
{
    public interface ITeamInvitationService
    {
        Task<OperationResult<List<TeamInvitation>>> GetForReceiverAsync(string userId);
        Task<OperationResult<List<TeamInvitationsShowViewModel>>> GetReceivedForShow(string userId);
        Task<OperationResult<List<TeamInvitation>>> GetForSenderAsync(string userId);
        Task<OperationResult<List<TeamInvitationsShowViewModel>>> GetSentForShow(string userId);
        Task<OperationResult<bool>> IsInvited(string userName, int teamId);
        Task<OperationResult<TeamInvitation>> SendAsync(InvitationViewModel invitationToSend);
        Task<OperationResult> EditAsync(TeamInvitationEditMessageViewModel invitationToUpdate);
        Task<OperationResult> AcceptAsync(int id, string userId);
        Task<OperationResult> RejectAsync(int id, string userId);
        Task<OperationResult> CancelAsync(int id);
    }
}
