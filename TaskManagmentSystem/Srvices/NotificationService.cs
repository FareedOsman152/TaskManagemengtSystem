using Microsoft.EntityFrameworkCore;
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

        public NotificationService(INotificationRepository notifiRepository, INotificationScheduler notifiScheduler, INotificationDispatcher notifiDispatcher, IUserService userService)
        {
            _notifiRepository = notifiRepository;
            _notifiScheduler = notifiScheduler;
            _notifiDispatcher = notifiDispatcher;
            _userService = userService;
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
                if(notifications.Count>0)
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

            var sender = await _userService.GetByIdAsync(invitation.SenderId);
            var receiver = await _userService.GetByIdAsync(invitation.ReceiverId);

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
            if(!createResult.Succeeded)
                return OperationResult.Failure(createResult.ErrorMessage);
            try
            {
                await _notifiDispatcher.SendNotificationAsync(new NotificationForSentViewModel
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
            ;
        
        }
    }
}
