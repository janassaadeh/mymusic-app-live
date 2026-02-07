using System.Text.Json.Serialization;

namespace mymusic_app.Models
{
    public class Song
    {
        public Guid Id { get; set; }
        public string DeezerTrackId { get; set; }
        public string Title { get; set; }
        public TimeSpan Duration { get; set; }

        public Guid ArtistId { get; set; }
        public Artist Artist { get; set; }

        public Guid AlbumId { get; set; }
        public Album Album { get; set; }

        public ICollection<SongGenre> Genres { get; set; }
        public ICollection<UserSongLike> Likes { get; set; }
        public ICollection<UserSongPlay> Plays { get; set; }

        [JsonIgnore]
        public ICollection<PlaylistSong> Playlists { get; set; }
    }

}
