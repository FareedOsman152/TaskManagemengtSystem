using Microsoft.AspNetCore.SignalR;

namespace TaskManagmentSystem.Hubs
{
    public class NotificationsHub : Hub
    {
        public async Task SendNotification()
        {
            await Clients.Caller.SendAsync("NewNotification");
        }
    }
}
