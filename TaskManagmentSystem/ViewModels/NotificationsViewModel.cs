using Microsoft.AspNetCore.Authorization;
using TaskManagmentSystem.Models;

namespace TaskManagmentSystem.ViewModels
{
    public class NotificationsViewModel
    {
        public int Id { get; set; }
        public String Details { get; set; } = null!;
        public DateTime DateCeated { get; set; } = DateTime.Now;
        public bool IsRead { get; set; }

    }
}
