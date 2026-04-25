using System.ComponentModel.DataAnnotations;

namespace AnilistClone.DTOs.PostDTOs
{
    public class SearchRequestDTO
    {
        [MinLength(1, ErrorMessage = "Search term must be at least 1 character")]
        [MaxLength(100, ErrorMessage = "Search term cannot exceed 100 characters")]
        public string? searchTerm { get; set; }
    }
}
