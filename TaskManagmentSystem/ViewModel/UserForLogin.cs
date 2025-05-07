using System.ComponentModel.DataAnnotations;

namespace TaskManagmentSystem.ViewModel
{
    public class UserForLogin
    {
        [Required]
        [MinLength(7)]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [MinLength(7)]
        public string Password { get; set; }

        [Display(Name = "Remember me")]
        public bool RememberMe{ get; set; }
    }
}
