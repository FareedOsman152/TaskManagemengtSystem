using System.ComponentModel.DataAnnotations;

namespace TaskManagmentSystem.ViewModels
{
    public class UserRegisterViewModel
    {
        [RegularExpression(@"^[\w\.-]+@[\w\.-]+\.\w+$")]
        [Required]
        [Display(Name ="Email" )]
        public string Email { get; set; }

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

        [Required]
        [MinLength(5)]
        [MaxLength(20)]
        [Display(Name ="Username")]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [MinLength(6)]
        [MaxLength(20)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password")]
        [Display(Name ="Confirm Password")]
        public string ConfirmPassword { get; set; }
    }
}
