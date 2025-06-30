using TaskManagmentSystem.Helpers;
using TaskManagmentSystem.Models;

namespace TaskManagmentSystem.Repositories.Interfaces
{
    public interface ITeamInvitationRepository
    {
        Task<OperationResult<TeamInvitation>> GetBuIdAsync(int id);
        Task<OperationResult<List<TeamInvitation>>> GetForReceiverAsync(string userId);
        Task<OperationResult<List<TeamInvitation>>> GetForSenderAsync(string userId);
        Task<OperationResult<TeamInvitation>> AddAsync(TeamInvitation invitation);
        Task<OperationResult> UpdateAsync(TeamInvitation invitationToUpdate);
        Task<OperationResult> UpdateStatusAsync(TeamInvitation invitationToUpdate);
        Task<OperationResult> DeleteAsync(int id);

    }
}
