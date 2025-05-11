using System.ComponentModel.DataAnnotations;

namespace TaskManagmentSystem.ViewModels
{
    public class WorkSpaceForEditViewModel
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Tilte")]
        public string Tilte { get; set; } = null!;

        [Display(Name = "Description")]
        public string? Description { get; set; }
    }
}
