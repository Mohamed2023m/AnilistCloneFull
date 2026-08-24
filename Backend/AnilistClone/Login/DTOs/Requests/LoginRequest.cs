using System.ComponentModel.DataAnnotations;

namespace AnilistClone.Login.DTOs.Requests
{
    public class LoginRequest
    {
        [Required]
        [MinLength(3, ErrorMessage = "Username must be at least 3 characters.")]
        [MaxLength(20, ErrorMessage = "Username must be at most 20 characters.")]
        public string Username { get; set; }

        [Required]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters.")]
        [MaxLength(100)]
        public string Password { get; set; }
    }
}
