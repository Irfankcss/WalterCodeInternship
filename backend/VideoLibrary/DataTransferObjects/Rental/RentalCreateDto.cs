using System.ComponentModel.DataAnnotations;

namespace VideoLibrary.DataTransferObjects.Rental
{
    public class RentalCreateDto
    {
        [Required(ErrorMessage = "Date is required")]
        public DateTime Date { get; set; }
        [Required(ErrorMessage = "Return date is required")]
        public DateTime ReturnDate { get; set; }
        [Required(ErrorMessage ="Borrowed to ID is required")]
        public int BorrowedToId { get; set; }
        [Required(ErrorMessage = "Movie copy ID is required")]
        public int MovieCopyId { get; set; }
        [Required(ErrorMessage = "Borrowed by ID is required")]
        public int BorrowedById { get; set; }
    }
}
