using VideoLibrary.DataTransferObjects.MovieCopy;
using VideoLibrary.Models;

namespace VideoLibrary.DataTransferObjects.Movie
{
    public class MovieDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Year { get; set; }
        public string? ImdbId { get; set; }
        public double? ImdbRating { get; set; }
        public string? Poster { get; set; }
        public string? Description { get; set; }

        public int DirectorId { get; set; }
        public Models.Director Director { get; set; }

        public int EditedById { get; set; }
        public string EditedByUsername { get; set; }

        public List<MovieCopyDto> MovieCopies { get; set; }
    }

}
