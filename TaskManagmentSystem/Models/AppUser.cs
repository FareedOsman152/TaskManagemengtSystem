using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace TaskManagmentSystem.Models
{
    public enum Genders
    {
        Male,
        Female
    }
    public class AppUser : IdentityUser
    {
        public List<WorkSpace> WorkSpaces { get; set; }
        public List<Notification> Notifications { get; set; }
        public DateOnly Birthday{ get; set; }
        public Genders Gender{ get; set; }
        public AppUserProfile Profile { get; set; } = null!;
    }
}
