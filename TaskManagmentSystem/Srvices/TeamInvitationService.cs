using TaskManagmentSystem.Helpers;
using TaskManagmentSystem.Models;
using TaskManagmentSystem.Repositories;
using TaskManagmentSystem.Srvices.Interfaces;
using TaskManagmentSystem.Srvicese;
using TaskManagmentSystem.ViewModels;

namespace TaskManagmentSystem.Srvices
{
    public class TeamInvitationService : ITeamInvitationService
    {
        private readonly TeamInvitationRepository _teamInvitationRepository;
        private readonly UserService _userService;
        private readonly TeamAppUserService _teamAppUserService;
        private readonly TeamService _teamService;
        public TeamInvitationService(TeamInvitationRepository teamInvitationRepository, UserService userService, TeamAppUserService teamAppUserService, TeamService teamService)
        {
            _teamInvitationRepository = teamInvitationRepository;
            _userService = userService;
            _teamAppUserService = teamAppUserService;
            _teamService = teamService;
        }
        public async Task<bool> IsInvited(string userName, int teamId)
        {
            var user = await _userService.GetByUserNameAsync(userName);
            var invitationsReceived = 
                await _teamInvitationRepository.GetForReceiverAsync(user.Id);
            if (invitationsReceived is null)
                return false;

            return invitationsReceived.FirstOrDefault(i => i.TeamId == teamId) is not null;
        }
        public async Task<OperationResult<List<TeamInvitation>>> GetForReceiverAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return OperationResult<List<TeamInvitation>>.Failure("user id is null");

            if(!await _userService.IsExistAsync(userId))
                return OperationResult<List<TeamInvitation>>.Failure("user is not found");

            var invitations = await _teamInvitationRepository.GetForReceiverAsync(userId);
            return OperationResult<List<TeamInvitation>>.Success(invitations);
        }

        public async Task<OperationResult<List<TeamInvitation>>> GetForSenderAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return OperationResult<List<TeamInvitation>>.Failure("user id is null");

            if (!await _userService.IsExistAsync(userId))
                return OperationResult<List<TeamInvitation>>.Failure("user is not found");

            var invitations = await _teamInvitationRepository.GetForSenderAsync(userId);
            return OperationResult<List<TeamInvitation>>.Success(invitations);
        }
        public async Task<OperationResult> Send(InvitationViewModel invitationToSend)
        {
            try
            {
                var checkTeamIsFound = await _checkTeam(invitationToSend.TeamId);
                if (!checkTeamIsFound.Succeeded)
                    return checkTeamIsFound;

                var checkReceiverResult = await _checkReceiver(invitationToSend.ReceiverUserName, invitationToSend.TeamId);
                if (!checkReceiverResult.Succeeded)
                    return checkReceiverResult;
                
                await _saveInvitation(invitationToSend);
                
                return OperationResult.Success();
            }
            catch (Exception ex)
            {
                return OperationResult.Failure(ex.Message);
            }
            
        }
        private async Task _saveInvitation(InvitationViewModel invitationToSend)
        {
            var receiver = await _userService.GetByUserNameAsync(invitationToSend.ReceiverUserName);

            await _teamInvitationRepository.AddAsync(new TeamInvitation
            {
                SenderId = invitationToSend.SenderId,
                ReceiverId = receiver.Id,
                Message = invitationToSend.Message,
                TeamId = invitationToSend.TeamId,
                Status = InvitationStatus.Pending
            });
        }
        private async Task<OperationResult> _checkTeam(int teamId)
        {
            var team =  await _teamService.GetByIdAsync(teamId);
            if (team == null)
                return OperationResult.Failure($"team with id: {teamId} is not found");
            return OperationResult.Success();
        }
        private async Task<OperationResult> _checkReceiver(string userName, int teamId)
        {
            if(string.IsNullOrEmpty(userName))
                return OperationResult.Failure("username is null or empty");

            var receiver = await _userService.GetByUserNameAsync(userName);

            // is exist?
            if (receiver is null)
                return OperationResult.Failure($"user {userName} is not found");
            
            // is already member in the team?
            if(await _teamAppUserService.IsMemberAsync(receiver.Id, teamId))
                return OperationResult.Failure($"this member {userName} is already member in this team");

            // is already invited?
            if (await IsInvited(userName, teamId))
                return OperationResult.Failure($"this member {userName} is already invied");

            return OperationResult.Success();
        }
        public async Task<OperationResult> ChangeMessageAsync(TeamInvitationEditMessageViewModel invitationToUpdate)
        {
            var invitation = await _teamInvitationRepository.GetBuIdAsync(invitationToUpdate.Id);
            if(invitation is null)
                return OperationResult.Failure($"Invitation with id: {invitationToUpdate.Id} is not found");

            invitation.Message = invitationToUpdate.Message;
            await _teamInvitationRepository.UpdateAsync(invitation);
            return OperationResult.Success();
        }
        public async Task<OperationResult> CancelAsync(int id)
        {
            var invitation = await _teamInvitationRepository.GetBuIdAsync(id);
            if (invitation is null)
                return OperationResult.Failure($"Invitation with id: {id} is not found");

            if (invitation.Status != InvitationStatus.Pending)
                return OperationResult.Failure($"Invitation with id: {id} is not pending, you can not delete");

            await _teamInvitationRepository.DeleteAsync(id);
            return OperationResult.Success();
        }
        /// <summary>
        /// accepts the invitation if the user is the receiver of the invitation and waiting for get permission to join the team.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<OperationResult> AcceptAsync(int id, string userId)
        {
            // Check if the user is the receiver of the invitation
            var user = await _userService.GetByIdAsync(userId);
            if (user is null)
                return OperationResult.Failure($"User with id: {userId} is not found");

            // check if the invitation exists
            var invitation = await _teamInvitationRepository.GetBuIdAsync(id);
            if (invitation is null)
                return OperationResult.Failure($"Invitation with id: {id} is not found");

            if(invitation.ReceiverId != user.Id)
                return OperationResult.Failure($"User with id: {userId} is not the receiver of this invitation");
            // check if the invitation is pending
            var checkInvitationStatus = _checkInvitationStatus(invitation);
            if (!checkInvitationStatus.Succeeded)
                return checkInvitationStatus;

            invitation.Status = InvitationStatus.Accepted;
            await _teamInvitationRepository.UpdateStatusAsync(invitation);
            return OperationResult.Success();
        }
        private OperationResult _checkInvitationStatus(TeamInvitation invitation)
        {
            if (invitation.Status != InvitationStatus.Pending)
            {
                if (invitation.Status == InvitationStatus.Accepted)
                    return OperationResult.Failure($"Invitation with id: {invitation.Id} is already accepted, you can not accept it again");

                if (invitation.Status == InvitationStatus.Rejected)
                    return OperationResult.Failure($"Invitation with id: {invitation.Id} is already rejected, you can not accept it again");
            }
            return OperationResult.Success();
        }
        public async Task<OperationResult> RejectAsync(int id, string userId)
        {
            // Check if the user is the receiver of the invitation
            var user = await _userService.GetByIdAsync(userId);
            if (user is null)
                return OperationResult.Failure($"User with id: {userId} is not found");

            // check if the invitation exists
            var invitation = await _teamInvitationRepository.GetBuIdAsync(id);
            if (invitation is null)
                return OperationResult.Failure($"Invitation with id: {id} is not found");

            if (invitation.ReceiverId != user.Id)
                return OperationResult.Failure($"User with id: {userId} is not the receiver of this invitation");
            // check if the invitation is pending

            var checkInvitationStatus = _checkInvitationStatus(invitation);
            if (!checkInvitationStatus.Succeeded)
                return checkInvitationStatus;

            invitation.Status = InvitationStatus.Rejected;
            await _teamInvitationRepository.UpdateStatusAsync(invitation);
            return OperationResult.Success();

        }
    }
}
