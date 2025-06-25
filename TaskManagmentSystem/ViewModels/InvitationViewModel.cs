using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using TaskManagmentSystem.Models;

namespace TaskManagmentSystem.ViewModels
{
    public class InvitationViewModel
    {
        [Required]
        [Display(Name = "Username")]
        public string ReceiverUserName { get; set; } = null!;

        [ValidateNever]
        public string SenderId { get; set; } = null!;

        [ValidateNever]
        public int TeamId { get; set; }
        public string? Message { get; set; }

        public TeamPermissions Permissions { get; set; } = TeamPermissions.None;
    }
}
