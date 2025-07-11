namespace VideoLibrary.DTOs
{
    public class RentalCreateDto
    {
        public DateTime Date { get; set; }
        public DateTime ReturnDate { get; set; }
        public int BorrowedToId { get; set; }
        public int MovieCopyId { get; set; }
        public int BorrowedById { get; set; }
    }
}
