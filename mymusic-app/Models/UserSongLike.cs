namespace mymusic_app.Models
{
    public class UserSongLike
    {
        public Guid UserId { get; set; }
        public User User { get; set; }

        public Guid SongId { get; set; }
        public Song Song { get; set; }

        public DateTime LikedAt { get; set; } = DateTime.UtcNow;
    }

}
