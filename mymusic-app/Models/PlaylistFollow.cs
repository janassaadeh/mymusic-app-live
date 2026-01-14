namespace mymusic_app.Models
{
    public class PlaylistFollow
    {
        public Guid UserId { get; set; }
        public User User { get; set; }

        public Guid PlaylistId { get; set; }
        public Playlist Playlist { get; set; }

        public DateTime FollowedAt { get; set; } = DateTime.UtcNow;
    }

}
