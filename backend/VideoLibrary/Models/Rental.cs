namespace VideoLibrary.Models
{
    public class Rental
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public DateTime ReturnDate { get; set; }
        public int BorrowedToId { get; set; }
        public User BorrowedTo { get; set; }
        public int MovieCopyId { get; set; }
        public MovieCopy MovieCopy { get; set; }
        public int BorrowedById { get; set; }
        public User BorrowedBy { get; set; }
    }
}
