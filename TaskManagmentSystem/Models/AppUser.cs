using Microsoft.AspNetCore.Identity;

namespace TaskManagmentSystem.Models
{
    public class AppUser : IdentityUser
    {
        public List<WorkSpace> WorkSpaces { get; set; }
        public List<Notification> Notifications { get; set; }
    }
}
