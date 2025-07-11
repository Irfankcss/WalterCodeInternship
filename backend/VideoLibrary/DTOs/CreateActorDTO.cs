using System.ComponentModel.DataAnnotations;

namespace VideoLibrary.DTOs
{
    public class CreateActorDto
    {
        public string Name { get; set; }
        public DateTime Dob { get; set; }
        public string Bio { get; set; }
    }
}