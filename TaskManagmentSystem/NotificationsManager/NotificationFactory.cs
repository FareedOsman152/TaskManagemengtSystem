using TaskManagmentSystem.Models;
using TaskManagmentSystem.NotificationsManager.Interfaces;

namespace TaskManagmentSystem.NotificationsManager
{
    public class NotificationFactory : INotificationFactory
    {
        public Notification TaskSchedule(string userId, int userTaskId, string details, bool IsRead = false)
        {
            return new Notification
            {
                AppUserId = userId,
                UserTaskId = userTaskId,
                Details = details,
                IsRead = IsRead
            };
        }
    }
}
