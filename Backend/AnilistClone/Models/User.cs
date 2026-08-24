using System.ComponentModel.DataAnnotations;
using AnilistClone.Models.Enums;

namespace AnilistClone.Models
{
    public class User
    {
        public int UserId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Username is required")]
        public required string Username { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Password is required")]
        public required string Password { get; set; }

        public UserType UserType { get; set; } = UserType.User;
    }
}
