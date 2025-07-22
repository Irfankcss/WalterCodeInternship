using System.ComponentModel.DataAnnotations;

namespace VideoLibrary.DataTransferObjects.Movie
{
    public class MovieCreateDto
    {
        [Required(ErrorMessage = "Movie name is required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Description is required")]
        public int Year { get; set; }
        [Required(ErrorMessage = "Year is required")]
        public string ImdbId { get; set; }
        [Required(ErrorMessage = "Description is required")]
        public double? ImdbRating { get; set; }
        [Required(ErrorMessage = "Poster URL is required")]
        public string Poster { get; set; }
        [Required(ErrorMessage = "Description is required")]
        public int DirectorId { get; set; }
        [Required(ErrorMessage = "Edited by user ID is required")]
        public int EditedById { get; set; }

        [Required(ErrorMessage = "At least one actor ID is required")]
        public List<int> ActorIds { get; set; } = new();
        [Required(ErrorMessage = "At least one genre ID is required")]
        public List<int> GenreIds { get; set; } = new();
    }
}
