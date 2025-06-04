using Microsoft.EntityFrameworkCore;
using TaskManagmentSystem.Models;
using TaskManagmentSystem.Notifications.Interfaces;

namespace TaskManagmentSystem.Notifications
{
    public class NotificationManager : INotificationManager
    {
        private readonly INotificationRepository _notifiRepository;
        private readonly INotificationScheduler _notifiScheduler;
        public NotificationManager(INotificationRepository notifiRepository, INotificationScheduler notifiScheduler)
        {
            _notifiRepository = notifiRepository;
            _notifiScheduler = notifiScheduler;
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
                    foreach (var n in notifications)
                    {
                        _notifiScheduler.SheduleTaskNotifiBeginOrEnd(n.DateToSend, userId);
                    }
                }
            }

        }
    }
}
