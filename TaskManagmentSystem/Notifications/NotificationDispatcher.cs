using Microsoft.AspNetCore.SignalR;
using TaskManagmentSystem.Hubs;
using TaskManagmentSystem.Models;
using TaskManagmentSystem.Notifications.Interfaces;
using TaskManagmentSystem.ViewModels;

namespace TaskManagmentSystem.Notifications
{
    public class NotificationDispatcher : INotificationDispatcher
    {
        private readonly IHubContext<NotificationsHub> _notificationHubContext;

        public NotificationDispatcher(IHubContext<NotificationsHub> notificationHubContext)
        {
            _notificationHubContext = notificationHubContext;
        }

        public async Task SendRealTimeTaskNotification(NotificationViewModel notification, string userId)
        {
            await _notificationHubContext.Clients.User(userId) 
                .SendAsync("ReceiveNewNotification", notification.Id,notification.Details,notification.IsRead,notification.TaskId,notification.DateToSend);
        }
    }
}
