using TaskManagmentSystem.Models;

namespace TaskManagmentSystem.Notifications.Interfaces
{
    public interface INotificationFactory
    {
        Notification TaskSchedule(string userId, int userTaskId, string details, DateTime dateToSend, bool IsRead = false);
    }
}
