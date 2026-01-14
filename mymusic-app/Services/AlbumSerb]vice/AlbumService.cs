using mymusic_app.Repositories;
using mymusic_app.Models;

namespace mymusic_app.Services
{
    public class AlbumService:IAlbumService
    {
        private readonly IAlbumRepository _albumRepo;

        public AlbumService(IAlbumRepository albumRepo)
        {
            _albumRepo = albumRepo;
        }

        // -----------------------------
        // Get by ID
        // -----------------------------
        public async Task<Album> GetByIdAsync(Guid albumId)
            => await _albumRepo.GetByIdAsync(albumId);

        // -----------------------------
        // Get all albums
        // -----------------------------
        public async Task<IEnumerable<Album>> GetAllAsync()
            => await _albumRepo.GetAllAsync();

        // -----------------------------
        // Get songs of album
        // -----------------------------
        public async Task<IEnumerable<Song>> GetAlbumSongsAsync(Guid albumId)
            => await _albumRepo.GetAlbumSongsAsync(albumId);

        // -----------------------------
        // Follow / Unfollow
        // -----------------------------
        public async Task FollowAlbumAsync(Guid albumId, Guid userId)
            => await _albumRepo.FollowAlbumAsync(albumId, userId);

        public async Task UnfollowAlbumAsync(Guid albumId, Guid userId)
            => await _albumRepo.UnfollowAlbumAsync(albumId, userId);

        // -----------------------------
        // Create album
        // -----------------------------
        public async Task<Album> CreateAsync(Album album)
        {
            return await _albumRepo.AddAsync(album);
        }

        // -----------------------------
        // Update album
        // -----------------------------
        public async Task UpdateAsync(Album album)
        {
            await _albumRepo.UpdateAsync(album);
        }

        // -----------------------------
        // Delete album
        // -----------------------------
        public async Task DeleteAsync(Guid albumId)
        {
            await _albumRepo.DeleteAsync(albumId);
        }
    }
}

