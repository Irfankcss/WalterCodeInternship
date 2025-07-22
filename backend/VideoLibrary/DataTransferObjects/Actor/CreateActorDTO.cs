using System.ComponentModel.DataAnnotations;

namespace VideoLibrary.DataTransferObjects.Actor
{
    public class CreateActorDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public DateTime Dob { get; set; }
        [Required]
        public string Bio { get; set; }
    }
}