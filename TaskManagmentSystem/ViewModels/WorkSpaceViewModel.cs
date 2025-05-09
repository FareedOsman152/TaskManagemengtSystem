using System.ComponentModel.DataAnnotations;

namespace TaskManagmentSystem.ViewModels
{
    public class WorkSpaceViewModel
    {
        [Required]
        public string Tilte { get; set; }
        public string? Discription { get; set; }
    }
}
