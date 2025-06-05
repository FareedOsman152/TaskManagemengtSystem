using Microsoft.AspNetCore.SignalR;

namespace TaskManagmentSystem.Hubs
{
    public class NotificationsHub : Hub
    {
        public async Task ThereIsANewNotification()
        {
            await Clients.Caller.SendAsync("NewNotification");
        }
    }
}
