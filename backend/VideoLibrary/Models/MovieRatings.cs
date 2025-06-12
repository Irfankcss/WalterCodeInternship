using System.ComponentModel.DataAnnotations.Schema;

namespace VideoLibrary.Models
{
    public class MovieRatings
    {
        public int MovieId { get; set; }
        public Movie Movie { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public int Rating { get; set; }
        public string Comment { get; set; }
    }
}
