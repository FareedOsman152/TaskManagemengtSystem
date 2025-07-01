using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using TaskManagmentSystem.Models;

namespace TaskManagmentSystem.ViewModels
{
    public class WorkSpaceViewModel
    {
        [Required]
        [Display(Name = "Title")]
        public string Title { get; set; } = null!;

        [Display(Name = "Description")]
        public string? Description { get; set; }
        public WorkSpaceColor Color { get; set; } = WorkSpaceColor.None;

        [ValidateNever]
        public List<string> Colors { get; set; }

        [HiddenInput]
        public int TeamId { get; set; }
    }
}
