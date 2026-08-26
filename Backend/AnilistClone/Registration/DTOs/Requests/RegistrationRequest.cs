using System.ComponentModel.DataAnnotations;

namespace AnilistClone.Registration.DTOs.Requests
{
    public class RegistrationRequest
    {
        [Required]
        [MinLength(3, ErrorMessage = "Username must be at least 3 characters.")]
        [MaxLength(20, ErrorMessage = "Username must be at most 20 characters.")]
        public string Username { get; set; }

        [Required]
        [MinLength(7, ErrorMessage = "Password must be at least 7 characters.")]
        [MaxLength(100)]
        public string Password { get; set; }
    }
}
