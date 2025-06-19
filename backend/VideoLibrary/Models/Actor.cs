using System.ComponentModel.DataAnnotations;

namespace VideoLibrary.Models
{
    public class Actor
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public DateTime Dob { get; set; }
        public string Bio { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeleteDate { get; set; }

    }
}
