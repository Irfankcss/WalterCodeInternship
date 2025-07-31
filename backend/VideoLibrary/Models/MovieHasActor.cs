namespace VideoLibrary.Models
{
    public class MovieHasActor
    {
        public int MovieId { get; set; }
        public Movie Movie { get; set; }
        public int ActorId { get; set; }
        public Actor Actor { get; set; }
        public bool MainActor { get; set; }
    }
}
