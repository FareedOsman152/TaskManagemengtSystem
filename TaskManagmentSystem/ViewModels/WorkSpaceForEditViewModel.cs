using System.ComponentModel.DataAnnotations;

namespace TaskManagmentSystem.ViewModels
{
    public class WorkSpaceForEditViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Tilte { get; set; }
        public string? Discription { get; set; }
    }
}
