using System.ComponentModel.DataAnnotations;

namespace VideoLibrary.DataTransferObjects.Director
{
    public class DirectorUpdateDto
    {
        [Required(ErrorMessage = "Director name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Date of birth is required")]
        public DateTime Dob { get; set; }

        [Required(ErrorMessage = "Biography is required")]
        public string Bio { get; set; }
    }
}
