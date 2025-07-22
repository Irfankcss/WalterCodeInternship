namespace VideoLibrary.DataTransferObjects.Movie
{
    // Moje misljenje, dovoljan DTO za update filma, jer ostali podaci su staticni i ne menjaju se osim usled greske
    public class MovieUpdateDto
    {
        public double? ImdbRating { get; set; }
        public string? Poster { get; set; }
    }
}