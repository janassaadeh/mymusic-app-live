namespace mymusic_app.Models
{
    public class UserSongPlay
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }

        public Guid SongId { get; set; }
        public Song Song { get; set; }

        public DateTime PlayedAt { get; set; } = DateTime.UtcNow;
    }

}
