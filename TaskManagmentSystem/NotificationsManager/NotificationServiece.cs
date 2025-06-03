using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TaskManagmentSystem.Models;
using TaskManagmentSystem.NotificationsManager.Interfaces;

namespace TaskManagmentSystem.NotificationsManager
{
    public class NotificationServiece : INotificationServiece
    {
        private readonly INotificationFactory _notificationFactory;
        public NotificationServiece(INotificationFactory notificationFactory)
        {
            _notificationFactory = notificationFactory;
        }

        public Notification BeginOn(string userId, UserTask userTask, bool IsRead = false)
        {
            var details = $"Your task \"${userTask.Title} Begins now ${userTask.BeginOn}";
            return _notificationFactory.TaskSchedule(userId, userTask.Id, details, IsRead);
        }

        public Notification EndOn(string userId, UserTask userTask, bool IsRead = false)
        {
            var details = $"Your task \"${userTask.Title} End now ${userTask.EndOn}";
            return _notificationFactory.TaskSchedule(userId, userTask.Id, details, IsRead);
        }

    }
}
