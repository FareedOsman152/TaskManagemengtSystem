namespace TaskManagmentSystem.Notifications.Interfaces
{
    public interface INotificationScheduler
    {
        void SheduleTaskNotifiBeginOrEnd(DateTime dateTime, string userId);
    }
}
