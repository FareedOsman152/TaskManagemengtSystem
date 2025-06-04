using Hangfire;
using Microsoft.AspNetCore.SignalR;
using TaskManagmentSystem.Hubs;
using TaskManagmentSystem.Notifications.Interfaces;

namespace TaskManagmentSystem.Notifications
{
    public class NotificationScheduler : INotificationScheduler
    {
        private readonly IHubContext<NotificationsHub> _notifiHubContext;
        private readonly IBackgroundJobClient _backgroundJobClient;
        public NotificationScheduler(IHubContext<NotificationsHub> notifiHubContext, IBackgroundJobClient backgroundJobClient)
        {
            _notifiHubContext = notifiHubContext;
            _backgroundJobClient = backgroundJobClient;
        }

        public void SheduleTaskNotifiBeginOrEnd(DateTime dateTime, string userId)
        {
            _backgroundJobClient.Schedule<IHubContext>(x => x.Clients.User(userId), dateTime);
        }
    }
}
