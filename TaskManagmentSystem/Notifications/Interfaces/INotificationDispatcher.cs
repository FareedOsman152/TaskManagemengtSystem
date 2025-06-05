﻿using TaskManagmentSystem.Models;
using TaskManagmentSystem.ViewModels;

namespace TaskManagmentSystem.Notifications.Interfaces
{
    public interface INotificationDispatcher
    {
        Task SendRealTimeTaskNotification(NotificationViewModel notification, string userId);
    }
}
