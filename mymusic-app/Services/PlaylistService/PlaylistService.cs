using mymusic_app.Models;
using mymusic_app.Repositories;

namespace mymusic_app.Services

{
    public class PlaylistService: IPlaylistService
    {
        private readonly IPlaylistRepository _playlistRepo;

        public PlaylistService(IPlaylistRepository playlistRepo)
        {
            _playlistRepo = playlistRepo;
        }

        public async Task<Playlist> GetByIdAsync(Guid playlistId)
        {
            return await _playlistRepo.GetByIdAsync(playlistId);
        }

        public async Task<IEnumerable<Playlist>> GetUserPlaylistsAsync(Guid userId)
            => await _playlistRepo.GetUserPlaylistsAsync(userId);

        public async Task<IEnumerable<Playlist>> GetFollowedPlaylistsAsync(Guid userId)
            => await _playlistRepo.GetFollowedPlaylistsAsync(userId);

        public async Task CreatePlaylistAsync(Playlist playlist)
            => await _playlistRepo.CreatePlaylistAsync(playlist);

        public async Task UpdatePlaylistAsync(Playlist playlist)
            => await _playlistRepo.UpdatePlaylistAsync(playlist);

        public async Task DeletePlaylistAsync(Guid playlistId)
            => await _playlistRepo.DeletePlaylistAsync(playlistId);

        public async Task AddSongAsync(Guid playlistId, Guid songId, int position)
            => await _playlistRepo.AddSongAsync(playlistId, songId, position);

        public async Task RemoveSongAsync(Guid playlistId, Guid songId)
            => await _playlistRepo.RemoveSongAsync(playlistId, songId);

        public async Task ReorderSongsAsync(Guid playlistId, List<Guid> newOrder)
            => await _playlistRepo.ReorderSongsAsync(playlistId, newOrder);

        public async Task FollowPlaylistAsync(Guid playlistId, Guid userId)
            => await _playlistRepo.FollowPlaylistAsync(playlistId, userId);

        public async Task UnfollowPlaylistAsync(Guid playlistId, Guid userId)
            => await _playlistRepo.UnfollowPlaylistAsync(playlistId, userId);

        public async Task<IEnumerable<Playlist>> GetAllPlaylistsAsync()
        {
            return await _playlistRepo.GetAllAsync();
        }


    }
}

