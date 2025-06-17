using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagmentSystem.ViewModels
{
    public class ProfileShowViewModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? PicURL { get; set; }
        public string? JopTitle { get; set; }
    }
}
