
using mymusic_app.Models;


namespace mymusic_app.Services
{
    public interface IPlaylistService
    {
        // -----------------------------
        // Get playlist by ID
        // -----------------------------
        Task<Playlist> GetByIdAsync(Guid playlistId);

        // -----------------------------
        // Get playlists of a user
        // -----------------------------
        Task<IEnumerable<Playlist>> GetUserPlaylistsAsync(Guid userId);

        // -----------------------------
        // Get playlists followed by a user
        // -----------------------------
        Task<IEnumerable<Playlist>> GetFollowedPlaylistsAsync(Guid userId);

        // -----------------------------
        // Create / Update / Delete playlist
        // -----------------------------
        Task CreatePlaylistAsync(Playlist playlist);
        Task UpdatePlaylistAsync(Playlist playlist);
        Task DeletePlaylistAsync(Guid playlistId);

        // -----------------------------
        // Manage songs in playlist
        // -----------------------------
        Task AddSongAsync(Guid playlistId, Guid songId, int position);
        Task RemoveSongAsync(Guid playlistId, Guid songId);
        Task ReorderSongsAsync(Guid playlistId, List<Guid> newOrder);

        // -----------------------------
        // Follow / Unfollow playlist
        // -----------------------------
        Task FollowPlaylistAsync(Guid playlistId, Guid userId);
        Task UnfollowPlaylistAsync(Guid playlistId, Guid userId);

        // -----------------------------
        // Get all playlists
        // -----------------------------
        Task<IEnumerable<Playlist>> GetAllPlaylistsAsync();
    }
}
