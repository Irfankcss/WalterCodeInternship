namespace VideoLibrary.DTOs
{
    public class RentalDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public DateTime? ReturnDate { get; set; }
        public int? BorrowedById { get; set; }
        public string BorrowedByName { get; set; }
        public int? BorrowedToId { get; set; }
        public string BorrowedToName { get; set; }
        public int? MovieCopyId { get; set; }
        public int? MovieId { get; set; }
        public string MovieTitle { get; set; }
    }
}
