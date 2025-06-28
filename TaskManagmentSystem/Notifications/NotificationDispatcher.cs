using Microsoft.AspNetCore.SignalR;
using TaskManagmentSystem.Controllers;
using TaskManagmentSystem.Hubs;
using TaskManagmentSystem.Models;
using TaskManagmentSystem.Notifications.Interfaces;
using TaskManagmentSystem.ViewModels;

namespace TaskManagmentSystem.Notifications
{
    public class NotificationDispatcher : INotificationDispatcher
    {
        private readonly IHubContext<NotificationsHub> _notificationHubContext;
        private readonly ILogger<HomeController> _logger;

        public NotificationDispatcher(IHubContext<NotificationsHub> notificationHubContext, ILogger<HomeController> logger)
        {
            _notificationHubContext = notificationHubContext;
            _logger = logger;
        }


        //public async Task SendRealTimeTaskNotification(NotificationViewModel notification, string userId)
        //{
        //    await _notificationHubContext.Clients.User(userId) 
        //        .SendAsync("ReceiveNewNotification", notification.Id,notification.Details,notification.IsRead,notification.TaskId,notification.DateToSend);
        //}

        public async Task SendNotificationAsync(NotificationForSentViewModel notification, string recipientId)
        {
            try
            {
                await _notificationHubContext.Clients.User(recipientId)
                    .SendAsync("ReceiveNotification", notification);

                _logger.LogInformation($"Notification sent to user {recipientId}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to send notification to user {recipientId}");
                throw; // أو معالجة الخطأ حسب احتياجاتك
            }
        }
    }
}
