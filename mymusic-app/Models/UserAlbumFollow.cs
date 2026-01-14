namespace mymusic_app.Models
{
    public class UserAlbumFollow
    {
        public Guid UserId { get; set; }
        public User User { get; set; }

        public Guid AlbumId { get; set; }
        public Album Album { get; set; }

        public DateTime FollowedAt { get; set; } = DateTime.UtcNow;
    }

}
