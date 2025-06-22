using TaskManagmentSystem.Models;

namespace TaskManagmentSystem.Repositories.Interfaces
{
    public interface ITeamInvitationRepository
    {
        Task<TeamInvitation> GetBuIdAsync(int id);
        Task<List<TeamInvitation>> GetForReceiverAsync(string userId);
        Task<List<TeamInvitation>> GetForSenderAsync(string userId);
        Task AddAsync(TeamInvitation invitation);
        Task UpdateAsync(TeamInvitation invitationToUpdate);
        Task UpdateStatusAsync(TeamInvitation invitationToUpdate);
        Task DeleteAsync(int id);

    }
}
