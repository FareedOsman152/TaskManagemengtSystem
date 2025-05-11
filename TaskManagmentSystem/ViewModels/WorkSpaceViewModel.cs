using System.ComponentModel.DataAnnotations;

namespace TaskManagmentSystem.ViewModels
{
    public class WorkSpaceViewModel
    {
        [Required]
        [Display(Name = "Tilte")]
        public string Tilte { get; set; } = null!;

        [Display(Name = "Discription")]
        public string? Discription { get; set; }
    }
}
