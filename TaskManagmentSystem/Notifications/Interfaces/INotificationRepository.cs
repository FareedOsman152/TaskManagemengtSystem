using TaskManagmentSystem.Models;

namespace TaskManagmentSystem.Notifications.Interfaces
{
    public interface INotificationRepository
    {
        Task<Notification> BeginAsync(string userId, UserTask userTask, bool IsRead = false);
        Task<Notification> BeforeBeginAsync(string userId, UserTask userTask, DateTime timeBeforeBegin, bool IsRead = false);
        Task<Notification> EndAsync(string userId, UserTask userTask, bool IsRead = false);
        Task<Notification> BeforeEndAsync(string userId, UserTask userTask, DateTime timeBeforeEnd, bool IsRead = false);
    }
}
