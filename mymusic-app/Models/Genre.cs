namespace mymusic_app.Models
{
    public class Genre
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public ICollection<ArtistGenre> Artists { get; set; }
        public ICollection<SongGenre> Songs { get; set; }
    }

}
