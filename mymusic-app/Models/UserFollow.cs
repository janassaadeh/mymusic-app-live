namespace mymusic_app.Models
{
    public class UserFollow
    {
        // Composite key (configured in OnModelCreating)
        public Guid FollowerId { get; set; }
        public User Follower { get; set; }

        public Guid FollowingId { get; set; }
        public User Following { get; set; }

        public DateTime FollowedAt { get; set; } = DateTime.UtcNow;
    }
}
