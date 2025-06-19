using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VideoLibrary.Models
{
    public class Movie
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public int Year { get; set; }
        public string? ImdbId { get; set; }
        public double? ImdbRating { get; set; }
        public string? Poster { get; set; }
        [ForeignKey(nameof(DirectorId))]
        public int DirectorId { get; set; }
        public Director Director { get; set; }
        [ForeignKey(nameof(EditedById))]
        public int EditedById { get; set; }
        public User EditedBy { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeleteDate { get; set; }
    }
}
