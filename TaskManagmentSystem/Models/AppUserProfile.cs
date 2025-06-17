using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagmentSystem.Models
{
    public class AppUserProfile
    {
        public int Id { get; set; }
        public string? PicURL { get; set; }
        public string? JopTitle { get; set; }

        [ForeignKey("AppUser")]
        public string AppUserId { get; set; } = null!;
        public AppUser AppUser { get; set; } = null!;

    }
}
