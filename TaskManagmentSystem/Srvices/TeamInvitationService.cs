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

        public async Task<bool> IsInvited(string userName, int teamId)
        {
            var user = await _userService.GetByUserNameAsync(userName);
            var invitationsReceived = 
                await _teamInvitationRepository.GetForReceiverAsync(user.Id);
            if (invitationsReceived is null)
                return false;

            return invitationsReceived.FirstOrDefault(i => i.TeamId == teamId) is not null;
        }
        //public async Task
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
        public async Task<OperationResult<List<TeamInvitationsShowViewModel>>> GetReceivedForShow(string userId)
        {
            var invitationsResult = await GetForReceiverAsync(userId);
            if (!invitationsResult.Succeeded)
                return OperationResult<List<TeamInvitationsShowViewModel>>.Failure(invitationsResult.ErrorMessage);

            var receiver = await _userService.GetByIdAsync(userId);

            var invitationViewModel = new List<TeamInvitationsShowViewModel>();
            foreach (var i in invitationsResult.Data)
            {
                var sender = await _userService.GetByIdAsync(i.SenderId);
                var team = await _teamService.GetByIdAsync(i.TeamId);
                if (sender is null || team is null)
                    continue; // skip this invitation if sender or team is not found
                //var message = _getMessageForReceiver(i.Message!, sender.UserName!, team.Title);
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
        //private string _getMessageForReceiver(string message, string senderUserName, string teamName)
        //{
        //    if (string.IsNullOrEmpty(message))
        //        return $"{senderUserName} has invited you to join the team {teamName}";
        //    return $"{senderUserName} has invited you to join the team {teamName} with message: {message}";
        //}
        public async Task<OperationResult<List<TeamInvitationsShowViewModel>>> GetSentForShow(string userId)
        {
            var invitationsResult = await GetForSenderAsync(userId);
            if (!invitationsResult.Succeeded)
                return OperationResult<List<TeamInvitationsShowViewModel>>.Failure(invitationsResult.ErrorMessage);

            var Sender = await _userService.GetByIdAsync(userId);

            var invitationViewModel = new List<TeamInvitationsShowViewModel>();
            foreach (var i in invitationsResult.Data)
            {
                var receiver = await _userService.GetByIdAsync(i.ReceiverId);
                var team = await _teamService.GetByIdAsync(i.TeamId);
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
        //private string _getMessageForSender(string message, string receiverUserName, string teamName)
        //{
        //    if (string.IsNullOrEmpty(message))
        //        return $"You has invited {receiverUserName} to join the team {teamName}";
        //    return $"You has invited {receiverUserName} to join the team {teamName} with message: {message}";
        //}
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
        private async Task<OperationResult<TeamInvitation>> _saveInvitation(InvitationViewModel invitationToSend)
        {
            var receiver = await _userService.GetByUserNameAsync(invitationToSend.ReceiverUserName);

            try
            {
                var invitation = await _teamInvitationRepository.AddAsync(new TeamInvitation
                {
                    SenderId = invitationToSend.SenderId,
                    ReceiverId = receiver.Id,
                    Message = invitationToSend.Message,
                    TeamId = invitationToSend.TeamId,
                    Status = InvitationStatus.Pending,
                    Permissions = invitationToSend.Permissions,
                    SendOn = DateTime.Now
                });
                return OperationResult<TeamInvitation>.Success(invitation);
            }
            catch (Exception ex)
            {

                return OperationResult<TeamInvitation>.Failure(ex.Message);
            }
           

        }
        private async Task<OperationResult> _checkTeam(int teamId)
        {
            var team =  await _teamService.GetByIdAsync(teamId);
            if (team == null)
                return OperationResult.Failure($"team with id: {teamId} is not found");
            return OperationResult.Success();
        }
        private async Task<OperationResult> _checkReceiver(string userName, int teamId, string senderId)
        {
            if(string.IsNullOrEmpty(userName))
                return OperationResult.Failure("username is null or empty");

            var receiver = await _userService.GetByUserNameAsync(userName);

            // is exist?
            if (receiver is null)
                return OperationResult.Failure($"user {userName} is not found");

            // is receiver the sender?
            if (receiver.Id == senderId)
                return OperationResult.Failure($"you can not invite yourself to the team");

            // is already member in the team?
            if (await _teamAppUserService.IsMemberAsync(receiver.Id, teamId))
                return OperationResult.Failure($"this member {userName} is already member in this team");

            // is already invited?
            if (await IsInvited(userName, teamId))
                return OperationResult.Failure($"this member {userName} is already invied");

            return OperationResult.Success();
        }
        public async Task<OperationResult> EditAsync(TeamInvitationEditMessageViewModel invitationToUpdate)
        {
            var invitation = await _teamInvitationRepository.GetBuIdAsync(invitationToUpdate.Id);
            if(invitation is null)
                return OperationResult.Failure($"Invitation with id: {invitationToUpdate.Id} is not found");

            invitation.Message = invitationToUpdate.Message;
            invitation.Permissions = invitationToUpdate.Permissions;
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

            await _teamAppUserService.AddAsync( user.Id, invitation.TeamId, invitation.Permissions);

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
            var notificationResult = await _notificationService.SendTeamInvitationAccepted(invitation,false);
            if (!notificationResult.Succeeded)
                return OperationResult.Failure(notificationResult.ErrorMessage);

            return OperationResult.Success();

        }
    }
}
