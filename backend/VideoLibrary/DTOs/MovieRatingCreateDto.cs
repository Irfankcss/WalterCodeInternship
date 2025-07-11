namespace VideoLibrary.DTOs
{
    public class MovieRatingCreateDto
    {
        public int MovieId { get; set; }
        public int UserId { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
    }
}
