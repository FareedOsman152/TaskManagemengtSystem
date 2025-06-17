using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace TaskManagmentSystem.ViewModels
{
    public class ProfilrEditViewModel
    {
        public int Id { get; set; }

        [Required]
        [MinLength(5)]
        [MaxLength(30)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [MinLength(5)]
        [MaxLength(30)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [MinLength(5)]
        [MaxLength(30)]
        [Display(Name = "Jop Title")]
        public string? JopTitle { get; set; }
        [ValidateNever]
        public string PicURL { get; set; }
        public IFormFile? ProfileImage{ get; set; }
    }
}
