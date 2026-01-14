namespace mymusic_app.Models
{
    public class ArtistGenre
    {
        public Guid ArtistId { get; set; }
        public Artist Artist { get; set; }

        public Guid GenreId { get; set; }
        public Genre Genre { get; set; }
    }

}
