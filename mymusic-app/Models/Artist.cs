namespace mymusic_app.Models
{
    public class Artist
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
        public string ImageUrl { get; set; }

        public ICollection<ArtistGenre> Genres { get; set; }
        public ICollection<Album> Albums { get; set; }
        public ICollection<Song> Songs { get; set; }

        public ICollection<UserArtistFollow> Followers { get; set; }
    }

}
