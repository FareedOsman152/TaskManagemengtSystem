using TaskManagmentSystem.Models;

namespace TaskManagmentSystem.NotificationsManager.Interfaces
{
    public interface INotificationFactory
    {
        Notification TaskSchedule(string userId, int userTaskId, string details, bool IsRead = false);
    }
}
