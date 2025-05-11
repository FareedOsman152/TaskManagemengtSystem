using System.ComponentModel.DataAnnotations;

namespace TaskManagmentSystem.ViewModels
{
    public class UserForLogin
    {
        [Required]
        [MinLength(5)]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [MinLength(6)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me")]
        public bool RememberMe{ get; set; }
    }
}
