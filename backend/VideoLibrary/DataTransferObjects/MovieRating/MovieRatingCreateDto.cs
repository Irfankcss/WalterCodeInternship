using System.ComponentModel.DataAnnotations;

namespace VideoLibrary.DataTransferObjects.MovieRating
{
    public class MovieRatingCreateDto
    {
        [Required(ErrorMessage = "Movie ID is required")]
        public int MovieId { get; set; }
        [Required(ErrorMessage = "User ID is required")]
        public int UserId { get; set; }
        [Required(ErrorMessage = "Rating is required")]
        public int Rating { get; set; }
        [Required(ErrorMessage = "Comment is required")]
        public string? Comment { get; set; }
    }
}
