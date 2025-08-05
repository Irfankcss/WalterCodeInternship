using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VideoLibrary.Models
{
    public class MovieRating
    {
        public int MovieId { get; set; }
        public Movie? Movie { get; set; }

        public int UserId { get; set; }
        public User? User { get; set; }

        [Required]
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeleteDate { get; set; }
    }
}
