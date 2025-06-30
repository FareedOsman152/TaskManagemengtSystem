using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using NuGet.Protocol.Plugins;
using TaskManagmentSystem.Helpers;
using TaskManagmentSystem.Models;
using TaskManagmentSystem.Repositories.Interfaces;
using TaskManagmentSystem.Srvices;
using TaskManagmentSystem.Srvices.Interfaces;

namespace TaskManagmentSystem.Repositories
{
    public class TeamInvitationRepository : ITeamInvitationRepository
    {
        private readonly AppDbContext _context;
        private readonly IUserService _userService;
        private readonly ILogger<TeamInvitationRepository> _logger;

        public TeamInvitationRepository(AppDbContext context,IUserService userService, ILogger<TeamInvitationRepository> logger)
        {
            _context = context;
            _userService = userService;
            _logger = logger;
        }

        public async Task<OperationResult<TeamInvitation>> GetBuIdAsync(int id)
        {
            var invitation = await _context.TeamInvitations.FindAsync(id);
            if(invitation is null)
                return OperationResult<TeamInvitation>.Failure("Invitation not found");
            return OperationResult<TeamInvitation>.Success(invitation);
        }

        public async Task<OperationResult<List<TeamInvitation>>> GetForReceiverAsync(string receiverId)
        {
            if (string.IsNullOrEmpty(receiverId))
                return OperationResult<List<TeamInvitation>>.Failure("Receiver ID is null");

           var isreceiverExistResult = await _userService.IsExistAsync(receiverId);
            if (!isreceiverExistResult.Succeeded)
                return OperationResult<List<TeamInvitation>>.Failure("Receiver does not exist");

            var invitations = await _context.TeamInvitations.
                Where(ti => ti.ReceiverId == receiverId)
                .ToListAsync();
            if(invitations is null)
                return OperationResult<List<TeamInvitation>>.Success(new List<TeamInvitation>());

            return OperationResult<List<TeamInvitation>>.Success(invitations);

        }

        public async Task<OperationResult<List<TeamInvitation>>> GetForSenderAsync(string senderId)
        {
            if (string.IsNullOrEmpty(senderId))
                return OperationResult<List<TeamInvitation>>.Failure("Sender ID is null");

            var isSenderExistResult = await _userService.IsExistAsync(senderId);
            if (!isSenderExistResult.Succeeded)
                return OperationResult<List<TeamInvitation>>.Failure("Sender does not exist");

            var invitations = await _context.TeamInvitations.
               Where(ti => ti.SenderId == senderId)
               .ToListAsync();

            if (invitations is null)
                return OperationResult<List<TeamInvitation>>.Success(new List<TeamInvitation>());

            return OperationResult<List<TeamInvitation>>.Success(invitations);
        }

        public async Task<OperationResult<TeamInvitation>> AddAsync(TeamInvitation invitation)
        {
            if (invitation is null)
                return OperationResult<TeamInvitation>.Failure("Invitation cannot be null");
            if (string.IsNullOrEmpty(invitation.SenderId) || string.IsNullOrEmpty(invitation.ReceiverId))
                return OperationResult<TeamInvitation>.Failure("Sender or Receiver ID cannot be null or empty");

            var isSenderExistResult = await _userService.IsExistAsync(invitation.SenderId);
            if (!isSenderExistResult.Succeeded)
                return OperationResult<TeamInvitation>.Failure("Sender does not exist");

            var isReceiverExistResult = await _userService.IsExistAsync(invitation.ReceiverId);
            if (!isReceiverExistResult.Succeeded)
                return OperationResult<TeamInvitation>.Failure("Receiver does not exist");

            try
            {
                await _context.TeamInvitations.AddAsync(invitation);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding Team Invitation from {SenderId} to {ReceiverId}",
                    invitation.SenderId, invitation.ReceiverId);
                return OperationResult<TeamInvitation>.Failure("An error occurred while adding the invitation: " + ex.Message);
            }
            return OperationResult<TeamInvitation>.Success(invitation);
        }

        public async Task<OperationResult> DeleteAsync(int id)
        {
            var invitationResult = await GetBuIdAsync(id);
            if (!invitationResult.Succeeded)
                return OperationResult.Failure(invitationResult.ErrorMessage);

            var invitation = invitationResult.Data;
            try
            {
                _context.TeamInvitations.Remove(invitation);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting Team Invitation with ID: {Id}", id);
                return OperationResult.Failure("An error occurred while deleting the invitation: " + ex.Message);
            }
            return OperationResult.Success();
        }
       
        public async Task<OperationResult> UpdateAsync(TeamInvitation invitationToUpdate)
        {
            if (invitationToUpdate is null)
                return OperationResult.Failure("Invitation cannot be null");

            var invitationResult = await GetBuIdAsync(invitationToUpdate.Id);
            if (!invitationResult.Succeeded)
                return OperationResult.Failure(invitationResult.ErrorMessage);

            var invitation = invitationResult.Data;

            invitation.Message = invitationToUpdate.Message;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating Team Invitation with ID: {Id}", invitationToUpdate.Id);
                return OperationResult.Failure("An error occurred while updating the invitation: " + ex.Message);
            }
           return OperationResult.Success();
        }
        public async Task<OperationResult> UpdateStatusAsync(TeamInvitation invitationToUpdate)
        {
            if (invitationToUpdate is null)
                return OperationResult.Failure("Invitation cannot be null");

            var invitationResult = await GetBuIdAsync(invitationToUpdate.Id);
            if (!invitationResult.Succeeded)
                return OperationResult.Failure(invitationResult.ErrorMessage);

            var invitation = invitationResult.Data;
            invitation.Status = invitationToUpdate.Status;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating Team Invitation with ID: {Id}", invitationToUpdate.Id);
                return OperationResult.Failure("An error occurred while updating the invitation: " + ex.Message);
            }
            return OperationResult.Success();
        }
    }
}
