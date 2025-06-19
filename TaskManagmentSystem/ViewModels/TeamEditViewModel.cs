using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace TaskManagmentSystem.ViewModels
{
    public class TeamEditViewModel
    {
        [ValidateNever]
        [HiddenInput]
        public int Id { get; set; }
        [Required]
        [MinLength(1)]
        [MaxLength(50)]
        public string Title { get; set; } = null!;

        [MinLength(1)]
        [MaxLength(255)]
        public string? Description { get; set; }
    }
}
