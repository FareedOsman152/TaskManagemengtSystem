using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using TaskManagmentSystem.Models;

namespace TaskManagmentSystem.ViewModels
{
    public class WorkSpaceViewModel
    {
        [Required]
        [Display(Name = "Tilte")]
        public string Tilte { get; set; } = null!;

        [Display(Name = "Description")]
        public string? Description { get; set; }
        public WorkSpaceColor Color { get; set; } = WorkSpaceColor.None;

        [ValidateNever]
        public List<string> Colors { get; set; }
    }
}
