using TaskManagmentSystem.Models;

namespace TaskManagmentSystem.Notifications.Interfaces
{
    public interface INotificationManager
    {
        Task ManageTaskBeginAndEndAsync(string userId, UserTask userTask);
    }
}
