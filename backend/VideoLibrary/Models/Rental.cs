using System.ComponentModel.DataAnnotations;

namespace VideoLibrary.Models
{
    public class Rental
    {
        public int Id { get; set; }
        [Required]
        public DateTime Date { get; set; }
        public DateTime? ReturnDate { get; set; }
        [Required]
        public int BorrowedToId { get; set; }
        public User BorrowedTo { get; set; }
        [Required]
        public int MovieCopyId { get; set; }
        public MovieCopy MovieCopy { get; set; }
        [Required]
        public int BorrowedById { get; set; }
        public User BorrowedBy { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeleteDate { get; set; }
    }
}
