using TaskManagmentSystem.Models;
using TaskManagmentSystem.ViewModels;

namespace TaskManagmentSystem.Notifications.Interfaces
{
    public interface INotificationScheduler
    {
        void SheduleTaskNotifiBeginOrEnd(List<NotificationViewModel> notifications, string userId);
    }
}
