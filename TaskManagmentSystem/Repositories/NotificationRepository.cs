using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TaskManagmentSystem.Helpers;
using TaskManagmentSystem.Models;
using TaskManagmentSystem.Notifications.Interfaces;
using TaskManagmentSystem.Repositories.Interfaces;

namespace TaskManagmentSystem.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly INotificationFactory _notificationFactory;
        private readonly AppDbContext _context;

        public NotificationRepository(INotificationFactory notificationFactory, AppDbContext context)
        {
            _notificationFactory = notificationFactory;
            _context = context;
        }

        private async Task<Notification> createNotificationAsync(string userId, int userTaskId,string details, DateTime dateToSend, bool IsRead)
        {
            var notifi = _notificationFactory.TaskSchedule(userId, userTaskId, details,dateToSend, IsRead);
            if (notifi is not null)
            {
                await _context.AddAsync(notifi);
                await _context.SaveChangesAsync();
            }
            return notifi;
        }
        public async Task<Notification> BeginAsync(string userId, UserTask userTask, bool IsRead = false)
        {
            var details = $"Your task ({userTask.Title}) Begins now  in {userTask.BeginOn}";
            return await createNotificationAsync(userId, userTask.Id,  details, userTask.BeginOn.Value, IsRead);
        }

        public async Task<Notification> BeforeBeginAsync(string userId, UserTask userTask, DateTime timeBeforeBegin, bool IsRead = false)
        {
            var details = $"Your task {userTask.Title} Begins After {userTask.BeginOn - timeBeforeBegin} in {userTask.BeginOn}";

            return await createNotificationAsync(userId, userTask.Id, details, timeBeforeBegin, IsRead);
        }

        public async Task<Notification> EndAsync(string userId, UserTask userTask, bool IsRead = false)
        {
            var details = $"Your task {userTask.Title} End now in {userTask.EndOn}";
            return await createNotificationAsync(userId, userTask.Id, details, userTask.EndOn.Value, IsRead);
        }

        public async Task<Notification> BeforeEndAsync(string userId, UserTask userTask, DateTime timeBeforeEnd, bool IsRead = false)
        {
            var details = $"Your task {userTask.Title} End After {userTask.EndOn - timeBeforeEnd} in {userTask.BeginOn}";
            return await createNotificationAsync(userId, userTask.Id, details, timeBeforeEnd, IsRead);
        }

        public async Task<OperationResult<Notification>> CreateAsync(Notification notification)
        {
            if (notification is null)
                return OperationResult<Notification>.Failure("notification us null");
            await _context.AddAsync(notification);
            await _context.SaveChangesAsync();
            return OperationResult<Notification>.Success(notification);
        }
    }
}
