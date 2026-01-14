using mymusic_app.Models;

namespace mymusic_app.Repositories
{
    public interface IUserRepository
    {
        // ---------------- USERS ----------------
        Task<User> GetByIdAsync(Guid id);
        Task<User> GetByEmailAsync(string email); // for login
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task<IEnumerable<User>> GetAllAsync();

        // ---------------- FOLLOWERS / FOLLOWING ----------------
        Task<int> GetFollowersCountAsync(Guid userId);
        Task<int> GetFollowingCountAsync(Guid userId);
        Task<IEnumerable<User>> GetFollowersAsync(Guid userId);
        Task<IEnumerable<User>> GetFollowingAsync(Guid userId);

        // ---------------- SONGS ----------------
        Task<IEnumerable<Song>> GetTopPlayedSongsAsync(Guid userId, int limit = 20);
        Task<int> GetLikesCountAsync(Guid userId);
        Task<IEnumerable<Song>> GetLikedSongsAsync(Guid userId);

        // ---------------- ARTISTS / ALBUMS ----------------
        Task<IEnumerable<Artist>> GetFollowedArtistsAsync(Guid userId);
        Task<IEnumerable<Album>> GetFollowedAlbumsAsync(Guid userId);

        // ---------------- PLAYLISTS ----------------
        Task<IEnumerable<Playlist>> GetUserPlaylistsAsync(Guid userId);
        Task<IEnumerable<Playlist>> GetFollowedPlaylistsAsync(Guid userId);

        // ---------------- ACTION METHODS ----------------
        Task LikeSongAsync(Guid userId, Guid songId);
        Task UnlikeSongAsync(Guid userId, Guid songId);

        Task FollowUserAsync(Guid userId, Guid followUserId);
        Task UnfollowUserAsync(Guid userId, Guid followUserId);

        Task FollowArtistAsync(Guid userId, Guid artistId);
        Task UnfollowArtistAsync(Guid userId, Guid artistId);

        Task FollowPlaylistAsync(Guid userId, Guid playlistId);
        Task UnfollowPlaylistAsync(Guid userId, Guid playlistId);
    }
}
