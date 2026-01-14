namespace mymusic_app.Models
{
    public class PlaylistSong
    {
        public Guid PlaylistId { get; set; }
        public Playlist Playlist { get; set; }

        public Guid SongId { get; set; }
        public Song Song { get; set; }

        public int Position { get; set; }
        public DateTime AddedAt { get; set; } = DateTime.UtcNow;
    }

}
