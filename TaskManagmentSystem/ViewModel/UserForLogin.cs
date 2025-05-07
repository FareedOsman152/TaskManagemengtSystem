using System.ComponentModel.DataAnnotations;

namespace TaskManagmentSystem.ViewModel
{
    public class UserForLogin
    {
        [Required]
        [MinLength(5)]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [MinLength(6)]
        public string Password { get; set; }

        [Display(Name = "Remember me")]
        public bool RememberMe{ get; set; }
    }
}
