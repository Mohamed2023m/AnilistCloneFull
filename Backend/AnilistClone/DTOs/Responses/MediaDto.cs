using AnilistClone.Models;

namespace AnilistClone.DTOs.GetDTOs
{
    public class MediaDto
    {
        public int Id { get; set; }
        public Title? Title { get; set; }
        public CoverImage? coverImage { get; set; }
    }
}
