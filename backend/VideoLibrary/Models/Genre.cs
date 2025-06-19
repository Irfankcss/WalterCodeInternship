using System.ComponentModel.DataAnnotations;

namespace VideoLibrary.Models
{
    public class Genre
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeleteDate { get; set; }

    }
}
