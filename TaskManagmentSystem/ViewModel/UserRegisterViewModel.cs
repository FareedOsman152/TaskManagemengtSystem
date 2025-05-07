using System.ComponentModel.DataAnnotations;

namespace TaskManagmentSystem.ViewModel
{
    public class UserRegisterViewModel
    {
        [RegularExpression(@"^[\w\.-]+@[\w\.-]+\.\w+$")]
        [Required]
        public string Email { get; set; }

        [Required]
        [MinLength(5)]
        [MaxLength(20)]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [MinLength(6)]
        [MaxLength(20)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}
