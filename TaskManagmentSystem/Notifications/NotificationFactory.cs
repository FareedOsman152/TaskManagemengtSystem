using TaskManagmentSystem.Models;
using TaskManagmentSystem.Notifications.Interfaces;

namespace TaskManagmentSystem.Notifications
{
    public class NotificationFactory : INotificationFactory
    {
        public Notification TaskSchedule(string userId, int userTaskId, string details,DateTime dateToSend, bool IsRead = false)
        {
            return new Notification
            {
                RecipientId = userId,
                UserTaskId = userTaskId,
                Details = details,
                IsRead = IsRead,
                DateToSend = dateToSend,
            };
        }
    }
}
