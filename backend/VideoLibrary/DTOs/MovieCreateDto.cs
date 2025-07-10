namespace VideoLibrary.DTOs
{
    public class MovieCreateDto
    {
        public string Name { get; set; }
        public int Year { get; set; }
        public string ImdbId { get; set; }
        public double? ImdbRating { get; set; }
        public string Poster { get; set; }

        public int DirectorId { get; set; }
        public int EditedById { get; set; }

        public List<int> ActorIds { get; set; } = new();
        public List<int> GenreIds { get; set; } = new();
    }
}
