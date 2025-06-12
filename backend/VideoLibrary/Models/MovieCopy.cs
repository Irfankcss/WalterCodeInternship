using System.ComponentModel.DataAnnotations.Schema;

namespace VideoLibrary.Models
{
    public class MovieCopy
    {
        public int Id { get; set; }
        [ForeignKey(nameof(MovieId))]
        public int MovieId { get; set; }
        public Movie Movie { get; set; }
        public string SerialNumber { get; set; }
        public string Description { get; set; }

    }
}
