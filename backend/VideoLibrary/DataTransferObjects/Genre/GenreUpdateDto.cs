using System.ComponentModel.DataAnnotations;

namespace VideoLibrary.DataTransferObjects.Genre
{
    public class GenreUpdateDto
    {
        [Required(ErrorMessage = "Genre name is required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }
    }
}
