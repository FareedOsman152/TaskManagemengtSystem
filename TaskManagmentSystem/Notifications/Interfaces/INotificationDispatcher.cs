using TaskManagmentSystem.Models;
using TaskManagmentSystem.ViewModels;

namespace TaskManagmentSystem.Notifications.Interfaces
{
    public interface INotificationDispatcher
    {
        Task SendNotificationAsync(NotificationForSentViewModel notification, string recipientId);
    }
}
