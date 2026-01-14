using mymusic_app.Models;

namespace mymusic_app.Repositories
{
    public interface IPlaylistRepository
    {
        Task<Playlist> GetByIdAsync(Guid playlistId);
        Task<IEnumerable<Playlist>> GetUserPlaylistsAsync(Guid userId);
        Task<IEnumerable<Playlist>> GetFollowedPlaylistsAsync(Guid userId);

        Task CreatePlaylistAsync(Playlist playlist);
        Task UpdatePlaylistAsync(Playlist playlist);
        Task DeletePlaylistAsync(Guid playlistId);

        Task AddSongAsync(Guid playlistId, Guid songId, int position);
        Task RemoveSongAsync(Guid playlistId, Guid songId);
        Task ReorderSongsAsync(Guid playlistId, List<Guid> newOrder);
        Task<IEnumerable<Playlist>> GetAllAsync();


        Task FollowPlaylistAsync(Guid playlistId, Guid userId);
        Task UnfollowPlaylistAsync(Guid playlistId, Guid userId);
    }
}
