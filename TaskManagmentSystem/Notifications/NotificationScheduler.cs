using Hangfire;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR;
using TaskManagmentSystem.Hubs;
using TaskManagmentSystem.Models;
using TaskManagmentSystem.Notifications.Interfaces;
using TaskManagmentSystem.ViewModels;

namespace TaskManagmentSystem.Notifications
{
    public class NotificationScheduler : INotificationScheduler
    {
        private readonly INotificationDispatcher _notificationDispatcher;
        private readonly IBackgroundJobClient _backgroundJob;
        public NotificationScheduler(INotificationDispatcher notificationDispatcher, IBackgroundJobClient backgroundJob)
        {
            _notificationDispatcher = notificationDispatcher;
            _backgroundJob = backgroundJob;
        }

        public void SheduleTaskNotifiBeginOrEnd(List<NotificationViewModel> notifications, string userId)
        {
            //foreach (var notification in notifications)
            //{
            //    _backgroundJob.Schedule(
            //() => _notificationDispatcher.SendRealTimeTaskNotification(notification, userId),
            //notification.DateToSend);
            //}            
        }
    }
}
