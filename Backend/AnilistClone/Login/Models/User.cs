using System.ComponentModel.DataAnnotations;

namespace AnilistClone.Login.Models
{
    public class User
    {
        public int UserId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Username is required")]
        public required string Username { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Password is required")]
        public required string Password { get; set; }

        [Required]
        public required string UserType { get; set; }
    }
}
