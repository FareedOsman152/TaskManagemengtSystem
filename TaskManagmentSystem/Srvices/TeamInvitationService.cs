using TaskManagmentSystem.Helpers;
using TaskManagmentSystem.Models;
using TaskManagmentSystem.Repositories;
using TaskManagmentSystem.Repositories.Interfaces;
using TaskManagmentSystem.Srvices.Interfaces;
using TaskManagmentSystem.Srvicese;
using TaskManagmentSystem.ViewModels;

namespace TaskManagmentSystem.Srvices
{
    public class TeamInvitationService : ITeamInvitationService
    {
        private readonly ITeamInvitationRepository _teamInvitationRepository;
        private readonly IUserService _userService;
        private readonly ITeamAppUserService _teamAppUserService;
        private readonly ITeamService _teamService;
        private readonly INotificationService _notificationService;

        public TeamInvitationService(ITeamInvitationRepository teamInvitationRepository, IUserService userService, ITeamAppUserService teamAppUserService, ITeamService teamService, INotificationService notificationService)
        {
            _teamInvitationRepository = teamInvitationRepository;
            _userService = userService;
            _teamAppUserService = teamAppUserService;
            _teamService = teamService;
            _notificationService = notificationService;
        }

        /// <summary>
        /// If the user is invited to the team or have rejected the invitation, it returns true, otherwise false.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="teamId"></param>
        /// <returns></returns>
        public async Task<OperationResult<bool>> IsInvited(string userName, int teamId)
        {
            var userResult = await _userService.GetByUserNameAsync(userName);
            if(!userResult.Succeeded)
                return OperationResult<bool>.Failure(userResult.ErrorMessage);

            var user = userResult.Data;
            var invitationsReceivedResult = 
                await _teamInvitationRepository.GetForReceiverAsync(user.Id);

            if(!invitationsReceivedResult.Succeeded)
                return OperationResult<bool>.Failure(invitationsReceivedResult.ErrorMessage);

            var invitationsReceived = invitationsReceivedResult.Data;
            var invitation =  invitationsReceived.FirstOrDefault(i => i.TeamId == teamId);

            if (invitation is null || invitation.Status == InvitationStatus.Rejected)
                return OperationResult<bool>.Success(false);

            return OperationResult<bool>.Success(true);
        }
        /// <summary>
        /// Get received invitations for a user.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<OperationResult<List<TeamInvitation>>> GetForReceiverAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return OperationResult<List<TeamInvitation>>.Failure("user id is null");

            var isUserExistResult = await _userService.IsExistAsync(userId);
            if (!isUserExistResult.Succeeded)
                return OperationResult<List<TeamInvitation>>.Failure("user is not found");

            var invitationsResult = await _teamInvitationRepository.GetForReceiverAsync(userId);
            if (!invitationsResult.Succeeded)
                return OperationResult<List<TeamInvitation>>.Failure(invitationsResult.ErrorMessage);

            var invitations = invitationsResult.Data;
            return OperationResult<List<TeamInvitation>>.Success(invitations);
        }

        /// <summary>
        /// Get sent invitations for a user.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<OperationResult<List<TeamInvitation>>> GetForSenderAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return OperationResult<List<TeamInvitation>>.Failure("user id is null");

            var isUserExistResult = await _userService.IsExistAsync(userId);
            if (!isUserExistResult.Succeeded)
                return OperationResult<List<TeamInvitation>>.Failure("user is not found");

            var invitationsResult = await _teamInvitationRepository.GetForSenderAsync(userId);
            if (!invitationsResult.Succeeded)
                return OperationResult<List<TeamInvitation>>.Failure(invitationsResult.ErrorMessage);

            var invitations = invitationsResult.Data;
            return OperationResult<List<TeamInvitation>>.Success(invitations);
        }

        /// <summary>
        /// Get received invitations for a user and convert them to view model for showing in the UI.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<OperationResult<List<TeamInvitationsShowViewModel>>> GetReceivedForShow(string userId)
        {
            var invitationsResult = await GetForReceiverAsync(userId);
            if (!invitationsResult.Succeeded)
                return OperationResult<List<TeamInvitationsShowViewModel>>.Failure(invitationsResult.ErrorMessage);

            var receiverResult = await _userService.GetByIdAsync(userId);
            if (!receiverResult.Succeeded)
                return OperationResult<List<TeamInvitationsShowViewModel>>.Failure(receiverResult.ErrorMessage);

            var receiver = receiverResult.Data;

            var invitationViewModel = new List<TeamInvitationsShowViewModel>();
            foreach (var i in invitationsResult.Data)
            {
                var senderResult = await _userService.GetByIdAsync(i.SenderId);
                if (!senderResult.Succeeded)
                    return OperationResult<List<TeamInvitationsShowViewModel>>.Failure(senderResult.ErrorMessage);

                var sender = senderResult.Data;
                var teamResult = await _teamService.GetByIdAsync(i.TeamId);
                if (!teamResult.Succeeded)
                    return OperationResult<List<TeamInvitationsShowViewModel>>.Failure(teamResult.ErrorMessage);

                var team = teamResult.Data;
                if (sender is null || team is null)
                    continue; // skip this invitation if sender or team is not found
                invitationViewModel.Add(new TeamInvitationsShowViewModel
                {
                    Id = i.Id,
                    OtherUserName = sender.UserName!,
                    TeamName = team.Title,
                    Message = i.Message,
                    Status = i.Status,
                    SendOn = i.SendOn
                });
            }

            return OperationResult<List<TeamInvitationsShowViewModel>>.Success(invitationViewModel.ToList());
        }

        /// <summary>
        /// Get sent invitations for a user and convert them to view model for showing in the UI.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<OperationResult<List<TeamInvitationsShowViewModel>>> GetSentForShow(string userId)
        {
            var invitationsResult = await GetForSenderAsync(userId);
            if (!invitationsResult.Succeeded)
                return OperationResult<List<TeamInvitationsShowViewModel>>.Failure(invitationsResult.ErrorMessage);

            var SenderResult = await _userService.GetByIdAsync(userId);
            if (!SenderResult.Succeeded)
                return OperationResult<List<TeamInvitationsShowViewModel>>.Failure(SenderResult.ErrorMessage);

            var sender = SenderResult.Data;

            var invitationViewModel = new List<TeamInvitationsShowViewModel>();
            foreach (var i in invitationsResult.Data)
            {
                var receiverResult = await _userService.GetByIdAsync(i.ReceiverId);
                if (!receiverResult.Succeeded)
                    return OperationResult<List<TeamInvitationsShowViewModel>>.Failure(receiverResult.ErrorMessage);

                var receiver = receiverResult.Data;
                var teamResult = await _teamService.GetByIdAsync(i.TeamId);
                if (!teamResult.Succeeded)
                    return OperationResult<List<TeamInvitationsShowViewModel>>.Failure(teamResult.ErrorMessage);

                var team = teamResult.Data;
                if (receiver is null || team is null)
                    continue; // skip this invitation if sender or team is not found
                //var message = _getMessageForSender(i.Message!, receiver.UserName!, team.Title);
                invitationViewModel.Add(new TeamInvitationsShowViewModel
                {
                    Id = i.Id,
                    OtherUserName = receiver.UserName!,
                    TeamName = team.Title,
                    Message = i.Message,
                    Status = i.Status,
                    SendOn = i.SendOn
                });
            }
            return OperationResult<List<TeamInvitationsShowViewModel>>.Success(invitationViewModel);
        }

        /// <summary>
        /// Sends an invitation to a user to join a team.
        /// Seneds a notification to the receiver if the invitation is sent successfully.
        /// </summary>
        /// <param name="invitationToSend"></param>
        /// <returns></returns>
        public async Task<OperationResult<TeamInvitation>> SendAsync(InvitationViewModel invitationToSend)
        {
            try
            {
                var checkTeamIsFound = await _checkTeam(invitationToSend.TeamId);
                if (!checkTeamIsFound.Succeeded)
                    return OperationResult<TeamInvitation>.Failure(checkTeamIsFound.ErrorMessage);

                var checkReceiverResult = await _checkReceiver(invitationToSend.ReceiverUserName, invitationToSend.TeamId, invitationToSend.SenderId);
                if (!checkReceiverResult.Succeeded)
                    return OperationResult<TeamInvitation>.Failure(checkReceiverResult.ErrorMessage); 
                
                var invitationResult = await _saveInvitation(invitationToSend);
                if(!invitationResult.Succeeded)
                    return OperationResult<TeamInvitation>.Failure(invitationResult.ErrorMessage);

                var notificationResult = await _notificationService.SendTeamInvitation(invitationResult.Data);
                if(!notificationResult.Succeeded)
                    return OperationResult<TeamInvitation>.Failure(notificationResult.ErrorMessage);

                return OperationResult<TeamInvitation>.Success(invitationResult.Data);
            }
            catch (Exception ex)
            {
                return OperationResult<TeamInvitation>.Failure(ex.Message);
            }
            
        }

        /// <summary>
        /// Saves the invitation to the database.
        /// </summary>
        /// <param name="invitationToSend"></param>
        /// <returns></returns>
        private async Task<OperationResult<TeamInvitation>> _saveInvitation(InvitationViewModel invitationToSend)
        {
            var receiverResult = await _userService.GetByUserNameAsync(invitationToSend.ReceiverUserName);
            if (!receiverResult.Succeeded)
                return OperationResult<TeamInvitation>.Failure(receiverResult.ErrorMessage);

            var receiver = receiverResult.Data;

            try
            {
                var invitationResult = await _teamInvitationRepository.AddAsync(new TeamInvitation
                {
                    SenderId = invitationToSend.SenderId,
                    ReceiverId = receiver.Id,
                    Message = invitationToSend.Message,
                    TeamId = invitationToSend.TeamId,
                    Status = InvitationStatus.Pending,
                    Permissions = invitationToSend.Permissions,
                    SendOn = DateTime.Now
                });
                if (!invitationResult.Succeeded)
                    return OperationResult<TeamInvitation>.Failure(invitationResult.ErrorMessage);

                var invitation = invitationResult.Data;
                return OperationResult<TeamInvitation>.Success(invitation);
            }
            catch (Exception ex)
            {

                return OperationResult<TeamInvitation>.Failure(ex.Message);
            }
           

        }
        private async Task<OperationResult> _checkTeam(int teamId)
        {
            var teamResult =  await _teamService.GetByIdAsync(teamId);
            if (!teamResult.Succeeded)
                return OperationResult.Failure(teamResult.ErrorMessage);

            var team = teamResult.Data;
            return OperationResult.Success();
        }

        /// <summary>
        /// Checks if the receiver is Exist, not the sender, not already a member of the team, and not already invited to the team.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="teamId"></param>
        /// <param name="senderId"></param>
        /// <returns></returns>
        private async Task<OperationResult> _checkReceiver(string userName, int teamId, string senderId)
        {
            if(string.IsNullOrEmpty(userName))
                return OperationResult.Failure("username is null or empty");

            var receiverResult = await _userService.GetByUserNameAsync(userName);
            if (!receiverResult.Succeeded)
                return OperationResult.Failure(receiverResult.ErrorMessage);

            var receiver = receiverResult.Data;

            // is receiver the sender?
            if (receiver.Id == senderId)
                return OperationResult.Failure($"you can not invite yourself to the team");

            // is already member in the team?
            var isMemberResult = await _teamAppUserService.IsMemberAsync(receiver.Id, teamId);
            if (!isMemberResult.Succeeded)
                return OperationResult.Failure(isMemberResult.ErrorMessage);

            if (isMemberResult.Data)
                return OperationResult.Failure($"this member {userName} is already a member of the team");

            // is already invited?
            var isInvitedResult = await IsInvited(userName, teamId);
            if (!isInvitedResult.Succeeded)
                return OperationResult.Failure(isInvitedResult.ErrorMessage);

            if(isInvitedResult.Data)
                return OperationResult.Failure($"this member {userName} is already invited to the team");
                        
            return OperationResult.Success();
        }
        public async Task<OperationResult> EditAsync(TeamInvitationEditMessageViewModel invitationToUpdate)
        {
            var invitationResult = await _teamInvitationRepository.GetBuIdAsync(invitationToUpdate.Id);
            if (!invitationResult.Succeeded)
                return OperationResult.Failure(invitationResult.ErrorMessage);

            var invitation = invitationResult.Data;

            invitation.Message = invitationToUpdate.Message;
            invitation.Permissions = invitationToUpdate.Permissions;
            var updateResult = await _teamInvitationRepository.UpdateAsync(invitation);
            if (!updateResult.Succeeded)
                return OperationResult.Failure(updateResult.ErrorMessage);

            return OperationResult.Success();
        }
        public async Task<OperationResult> CancelAsync(int id)
        {
            var invitationResult = await _teamInvitationRepository.GetBuIdAsync(id);
            if (!invitationResult.Succeeded)
                return OperationResult.Failure(invitationResult.ErrorMessage);

            var invitation = invitationResult.Data;

            if (invitation.Status != InvitationStatus.Pending)
                return OperationResult.Failure($"Invitation with id: {id} is not pending, you can not delete");

            var deleteResult = await _teamInvitationRepository.DeleteAsync(id);
            if (!deleteResult.Succeeded)
                return OperationResult.Failure(deleteResult.ErrorMessage);

            return OperationResult.Success();
        }

        /// <summary>
        /// accepts the invitation if the user is the receiver of the invitation.
        /// add user to the team.
        /// send notification the ivitation is accepted.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<OperationResult> AcceptAsync(int id, string userId)
        {
            // Check if the user is the receiver of the invitation
            var userResult = await _userService.GetByIdAsync(userId);
            if(!userResult.Succeeded)
                return OperationResult.Failure(userResult.ErrorMessage);

            var user = userResult.Data;
            // check if the invitation exists
            var invitationResult = await _teamInvitationRepository.GetBuIdAsync(id);
            if (!invitationResult.Succeeded)
                return OperationResult.Failure(invitationResult.ErrorMessage);
            var invitation = invitationResult.Data;
           
            if(invitation.ReceiverId != user.Id)
                return OperationResult.Failure($"User with id: {userId} is not the receiver of this invitation");
            // check if the invitation is pending
            var checkInvitationStatus = _checkInvitationStatus(invitation);
            if (!checkInvitationStatus.Succeeded)
                return checkInvitationStatus;

            invitation.Status = InvitationStatus.Accepted;
            var updateResult = await _teamInvitationRepository.UpdateStatusAsync(invitation);
            if (!updateResult.Succeeded)
                return OperationResult.Failure(updateResult.ErrorMessage);

            var addMemberToTeamMembers = await _teamAppUserService.AddAsync( user.Id, invitation.TeamId, invitation.Permissions);
            if (!addMemberToTeamMembers.Succeeded)
                return OperationResult.Failure(addMemberToTeamMembers.ErrorMessage);

            var notificationResult = await _notificationService.SendTeamInvitationAccepted(invitation);
            if (!notificationResult.Succeeded)
                return OperationResult.Failure(notificationResult.ErrorMessage);

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
        /// <summary>
        /// reject the invitation if the user is the receiver of the invitation.
        /// send notification the ivitation is rejected.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<OperationResult> RejectAsync(int id, string userId)
        {
            // Check if the user is the receiver of the invitation
            var userResult = await _userService.GetByIdAsync(userId);
            if (!userResult.Succeeded)
                return OperationResult.Failure(userResult.ErrorMessage);

            var user = userResult.Data;

            // check if the invitation exists
            var invitationResult = await _teamInvitationRepository.GetBuIdAsync(id);
            if (!invitationResult.Succeeded)
                return OperationResult.Failure(invitationResult.ErrorMessage);
            var invitation = invitationResult.Data;

            if (invitation.ReceiverId != user.Id)
                return OperationResult.Failure($"User with id: {userId} is not the receiver of this invitation");
            // check if the invitation is pending

            var checkInvitationStatusResult = _checkInvitationStatus(invitation);
            if (!checkInvitationStatusResult.Succeeded)
                return OperationResult.Failure(checkInvitationStatusResult.ErrorMessage);

            invitation.Status = InvitationStatus.Rejected;
            var updateResult = await _teamInvitationRepository.UpdateStatusAsync(invitation);
            if (!updateResult.Succeeded)
                return OperationResult.Failure(updateResult.ErrorMessage);

            var notificationResult = await _notificationService.SendTeamInvitationAccepted(invitation,false);
            if (!notificationResult.Succeeded)
                return OperationResult.Failure(notificationResult.ErrorMessage);

            return OperationResult.Success();

        }
    }
}
