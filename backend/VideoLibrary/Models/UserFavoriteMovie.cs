using System.ComponentModel.DataAnnotations.Schema;

namespace VideoLibrary.Models
{
    public class UserFavoriteMovie
    {
        public int UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }
        public int MovieId { get; set; }
        [ForeignKey(nameof(MovieId))]
        public Movie Movie { get; set; }

    }
}
