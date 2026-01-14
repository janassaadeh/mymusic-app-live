namespace mymusic_app.Models
{
    public class Album
    {
        public Guid Id { get; set; }

        public string Title { get; set; }
        public string CoverImageUrl { get; set; }
        public DateTime ReleaseDate { get; set; }

        public Guid ArtistId { get; set; }
        public Artist Artist { get; set; }

        public ICollection<Song> Songs { get; set; }
        public ICollection<UserAlbumFollow> Followers { get; set; }
    }

}
