using Newtonsoft.Json;

namespace AnilistClone.Models
{
    public class Show
    {
        public int Id { get; set; }

        public FuzzyDate? StartDate { get; set; }
        public FuzzyDate? EndDate { get; set; }
        public CoverImage? CoverImage { get; set; }


        public int? Episodes { get; set; }

        public List<string>? Genres { get; set; }
        public Title? Title { get; set; }
        public int? SeasonYear { get; set; }
        public string? Description { get; set; }
    }
}