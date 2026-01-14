namespace mymusic_app.Models
{
    public class SongGenre
    {
        public Guid SongId { get; set; }
        public Song Song { get; set; }

        public Guid GenreId { get; set; }
        public Genre Genre { get; set; }
    }

}
