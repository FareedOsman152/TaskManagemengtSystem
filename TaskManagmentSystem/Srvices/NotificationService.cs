using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
using TaskManagmentSystem.Helpers;
using TaskManagmentSystem.Models;
using TaskManagmentSystem.Notifications.Interfaces;
using TaskManagmentSystem.Repositories.Interfaces;
using TaskManagmentSystem.Srvices.Interfaces;
using TaskManagmentSystem.ViewModels;

namespace TaskManagmentSystem.Srvices
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notifiRepository;
        private readonly INotificationScheduler _notifiScheduler;
        private readonly INotificationDispatcher _notifiDispatcher;
        private readonly IUserService _userService;
        private readonly ITeamService _teamService;

        public NotificationService(INotificationRepository notifiRepository, INotificationScheduler notifiScheduler, INotificationDispatcher notifiDispatcher, IUserService userService, ITeamService teamService)
        {
            _notifiRepository = notifiRepository;
            _notifiScheduler = notifiScheduler;
            _notifiDispatcher = notifiDispatcher;
            _userService = userService;
            _teamService = teamService;
        }

        public async Task ManageTaskBeginAndEndAsync(string userId, UserTask userTask)
        {
            if (userTask is not null)
            {
                var notifications = new List<Notification>();
                // Create notifi Begin
                if (userTask.BeginOn is not null && userTask.BeginOn > DateTime.Now)
                {
                    var notifiBeginOn = await _notifiRepository.BeginAsync(userId!, userTask);
                    notifications.Add(notifiBeginOn);

                    if (userTask.RemindMeBeforeBegin is DateTime remindMeBeforeBegin && userTask.RemindMeBeforeBegin > DateTime.Now)
                    {
                        var notifiBeforeBegin = await _notifiRepository.BeforeBeginAsync(userId!, userTask, remindMeBeforeBegin);
                        notifications.Add(notifiBeforeBegin);
                    }
                }

                // Create notifi End
                if (userTask.EndOn is not null && userTask.EndOn > DateTime.Now)
                {
                    var notifiEndOn = await _notifiRepository.EndAsync(userId!, userTask);
                    notifications.Add(notifiEndOn);
                    if (userTask.RemindMeBeforeEnd is DateTime remindMeBeforeEnd && userTask.RemindMeBeforeEnd > DateTime.Now)
                    {
                        var notifiBeforeEnd = await _notifiRepository.BeforeEndAsync(userId!, userTask, remindMeBeforeEnd);
                        notifications.Add(notifiBeforeEnd);
                    }
                }
                if (notifications.Count > 0)
                {
                    var notificationsViewModel = new List<NotificationViewModel>();
                    foreach (var n in notifications)
                    {
                        notificationsViewModel.Add(new NotificationViewModel
                        {
                            Id = n.Id,
                            Details = n.Details,
                            DateToSend = n.DateToSend,
                            IsRead = n.IsRead,
                            TaskId = n.UserTaskId,
                        });
                    }
                    _notifiScheduler.SheduleTaskNotifiBeginOrEnd(notificationsViewModel, userId);
                }
            }

        }

        public async Task<OperationResult> SendTeamInvitation(TeamInvitation invitation)
        {
            if (invitation is null)
                return OperationResult.Failure("Invitation is null");

            var RecipientResult = await _userService.GetByIdAsync(invitation.ReceiverId);
            if (!RecipientResult.Succeeded)
                return OperationResult.Failure(RecipientResult.ErrorMessage);

            var Recipient = RecipientResult.Data;
            var createResult = await _createTeamInvitationNotification(invitation);
            if (!createResult.Succeeded)
                return OperationResult.Failure(createResult.ErrorMessage);

            var dispatchResult = _dispatchNotification(createResult.Data, Recipient);
            if (!dispatchResult.Succeeded)
                return OperationResult.Failure(dispatchResult.ErrorMessage);

            return OperationResult.Success();
        }
        /// <summary>
        /// Sends a notification when a team invitation is accepted or rejected .
        /// </summary>
        /// <param name="invitation"></param>
        /// <param name="accepted"></param>
        /// <returns></returns>
        public async Task<OperationResult> SendTeamInvitationAccepted(TeamInvitation invitation, bool accepted = true)
        {
            if (invitation is null)
                return OperationResult.Failure("Invitation is null");

            var RecipientResult = await _userService.GetByIdAsync(invitation.SenderId);
            if (!RecipientResult.Succeeded)
                return OperationResult.Failure(RecipientResult.ErrorMessage);

            var Recipient = RecipientResult.Data;
            var teamResult = await _teamService.GetByIdAsync(invitation.TeamId);
            if (!teamResult.Succeeded)
                return OperationResult.Failure(teamResult.ErrorMessage);

            invitation.Team = teamResult.Data;

            var createResult = await _createTeamInvitationAcceptedNotification(invitation,accepted);
            if (!createResult.Succeeded)
                return OperationResult.Failure(createResult.ErrorMessage);

            var dispatchResult = _dispatchNotification(createResult.Data, Recipient);
            if (!dispatchResult.Succeeded)
                return OperationResult.Failure(dispatchResult.ErrorMessage);

            return OperationResult.Success();
        }

        private async Task<OperationResult<Notification>> _createTeamInvitationAcceptedNotification(TeamInvitation invitation, bool accepted = true)
        {
            var senderResult = await _userService.GetByIdAsync(invitation.SenderId);
            if (!senderResult.Succeeded)
                return OperationResult<Notification>.Failure(senderResult.ErrorMessage);

            var sender = senderResult.Data;
            var receiverResult = await _userService.GetByIdAsync(invitation.ReceiverId);
            if (!receiverResult.Succeeded)
                return OperationResult<Notification>.Failure(receiverResult.ErrorMessage);

            var receiver = receiverResult.Data;
            var response = accepted ? "accepted" : "rejected";

            var notification = new Notification
            {
                Details = $"\"{receiver.UserName}\" {response} your invitation ti join to \"{invitation.Team.Title}\" team",
                IsRead = false,
                Type = NotificationType.TeamInvitationAccepted,
                DateToSend = DateTime.Now,
                RecipientId = invitation.SenderId,
                ActorId = invitation.ReceiverId,
                TeamInvitationId = invitation.Id
            };
            var createResult = await _notifiRepository.CreateAsync(notification);
            if (!createResult.Succeeded)
                return OperationResult<Notification>.Failure(createResult.ErrorMessage);

            return OperationResult<Notification>.Success(createResult.Data);
        }

        private async Task<OperationResult<Notification>> _createTeamInvitationNotification(TeamInvitation invitation)
        {
            var senderResult = await _userService.GetByIdAsync(invitation.SenderId);
            if (!senderResult.Succeeded)
                return OperationResult<Notification>.Failure(senderResult.ErrorMessage);

            var sender = senderResult.Data;
            var receiverResult = await _userService.GetByIdAsync(invitation.ReceiverId);
            if (!receiverResult.Succeeded)
                return OperationResult<Notification>.Failure(receiverResult.ErrorMessage);

            var receiver = receiverResult.Data;

            var notification = new Notification
            {
                Details = $"\"{sender.UserName}\" invited you to join to this tame \"{invitation.Team.Title}\"" +
                $" with message \"{invitation.Message}\"",
                IsRead = false,
                Type = NotificationType.TeamInvitationReceived,
                DateToSend = DateTime.Now,
                RecipientId = invitation.ReceiverId,
                ActorId = invitation.SenderId,
                TeamInvitationId = invitation.Id
            };
            var createResult = await _notifiRepository.CreateAsync(notification);
            if (!createResult.Succeeded)
                return OperationResult<Notification>.Failure(createResult.ErrorMessage);

            return OperationResult<Notification>.Success(createResult.Data);
        }
        private OperationResult _dispatchNotification(Notification notification, AppUser receiver)
        {
            try
            {
                _notifiDispatcher.SendNotificationAsync(new NotificationForSentViewModel
                {
                    Id = notification.Id,
                    Details = notification.Details,
                    IsRead = notification.IsRead,
                    DateToSend = notification.DateToSend
                }, receiver.Id);
                return OperationResult.Success();
            }
            catch (Exception ex)
            {
                return OperationResult.Failure($"Failed to send notification: {ex.Message}");
            }
        }
    }
}
