namespace mymusic_app.Models
{
    public class UserArtistFollow
    {
        public Guid UserId { get; set; }
        public User User { get; set; }

        public Guid ArtistId { get; set; }
        public Artist Artist { get; set; }

        public DateTime FollowedAt { get; set; } = DateTime.UtcNow;
    }

}
