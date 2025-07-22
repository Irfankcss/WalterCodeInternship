using System.ComponentModel.DataAnnotations;

namespace VideoLibrary.DataTransferObjects.Rental
{
    public class RentalUpdateDto
    {
        [Required]
        public DateTime Date { get; set; }
        public DateTime? ReturnDate { get; set; }
    }
}
