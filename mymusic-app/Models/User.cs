namespace mymusic_app.Models
{
    public class User
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Bio { get; set; }
        public bool IsAdmin { get; set; }


        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<UserFollow> Followers { get; set; }
        public ICollection<UserFollow> Following { get; set; }

        public ICollection<UserSongLike> LikedSongs { get; set; }
        public ICollection<UserAlbumFollow> FollowedAlbums { get; set; }
        public ICollection<UserArtistFollow> FollowedArtists { get; set; }

        public ICollection<Playlist> Playlists { get; set; }
        public ICollection<PlaylistFollow> FollowedPlaylists { get; set; }

        public ICollection<UserSongPlay> SongPlays { get; set; }
        public string PasswordHash { get; internal set; }
        public string Email { get; internal set; }
    }
}