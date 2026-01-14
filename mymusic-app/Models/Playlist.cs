namespace mymusic_app.Models
{
    public class Playlist
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public Guid OwnerId { get; set; }
        public User Owner { get; set; }

        public bool IsPublic { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<PlaylistSong> Songs { get; set; }
        public ICollection<PlaylistFollow> Followers { get; set; }
    }

}
