using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using NuGet.Protocol.Plugins;
using TaskManagmentSystem.Models;
using TaskManagmentSystem.Repositories.Interfaces;
using TaskManagmentSystem.Srvices;

namespace TaskManagmentSystem.Repositories
{
    public class TeamInvitationRepository : ITeamInvitationRepository
    {
        private readonly AppDbContext _context;

        public TeamInvitationRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<TeamInvitation> GetBuIdAsync(int id)
        {
            var invitation = await _context.TeamInvitations.FindAsync(id);

            return invitation;
        }

        public async Task<List<TeamInvitation>> GetForReceiverAsync(string receiverId)
        {
            var invitations = await _context.TeamInvitations.
                Where(ti => ti.ReceiverId == receiverId)
                .ToListAsync();
            return invitations;

        }

        public async Task<List<TeamInvitation>> GetForSenderAsync(string senderId)
        {
            var invitations = await _context.TeamInvitations.
               Where(ti => ti.SenderId == senderId)
               .ToListAsync();
            return invitations;
        }

        public async Task AddAsync(TeamInvitation invitation)
        {
            _context.TeamInvitations.AddAsync(invitation);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(TeamInvitation invitation)
        {
            _context.TeamInvitations.Remove(invitation);
            await _context.SaveChangesAsync();
        }
       
        public async Task UpdateAsync(TeamInvitation invitationToUpdate)
        {
            var invitation = await GetBuIdAsync(invitationToUpdate.Id);
            invitation.Message = invitationToUpdate.Message;
            await _context.SaveChangesAsync();
        }
        public async Task UpdateStatusAsync(TeamInvitation invitationToUpdate)
        {
            var invitation = await GetBuIdAsync(invitationToUpdate.Id);
            invitation.Status = invitationToUpdate.Status;
            await _context.SaveChangesAsync();
        }
    }
}
