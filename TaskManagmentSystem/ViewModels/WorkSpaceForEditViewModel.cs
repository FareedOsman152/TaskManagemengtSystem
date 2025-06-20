using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using TaskManagmentSystem.Models;

namespace TaskManagmentSystem.ViewModels
{
    public class WorkSpaceForEditViewModel
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Title")]
        public string Title { get; set; } = null!;

        [Display(Name = "Description")]
        public string? Description { get; set; }
        public WorkSpaceColor Color { get; set; } = WorkSpaceColor.None;

        [ValidateNever]
        public List<string> Colors { get; set; }
    }
}
