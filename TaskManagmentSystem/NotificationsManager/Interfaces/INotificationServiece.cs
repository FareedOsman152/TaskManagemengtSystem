using TaskManagmentSystem.Models;

namespace TaskManagmentSystem.NotificationsManager.Interfaces
{
    public interface INotificationServiece
    {
        Notification BeginOn(string userId, UserTask userTask, bool IsRead = false);
        Notification EndOn(string userId, UserTask userTask, bool IsRead = false);
    }
}
