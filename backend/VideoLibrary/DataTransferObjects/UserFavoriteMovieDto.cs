namespace VideoLibrary.DataTransferObjects
{
    internal class UserFavoriteMovieDto
    {
        public object Id { get; set; }
        public int MovieId { get; set; }
        public string MovieName { get; set; }
        public int MovieYear { get; set; }
        public string MoviePoster { get; set; }
    }
}