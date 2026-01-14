using mymusic_app.Repositories;
using mymusic_app.Models;

namespace mymusic_app.Services
{
    public class SongService:ISongService
    {
        private readonly ISongRepository _songRepo;

        public SongService(ISongRepository songRepo)
        {
            _songRepo = songRepo;
        }

        // -----------------------------
        // Get top songs by genre
        // -----------------------------
        public async Task<IEnumerable<Song>> GetTopSongsByGenreAsync(Guid genreId, int limit = 20)
        {
            return await _songRepo.GetTopSongsByGenreAsync(genreId, limit);
        }

        // -----------------------------
        // Get all songs
        // -----------------------------
        public async Task<IEnumerable<Song>> GetAllAsync()
        {
            return await _songRepo.GetAllAsync();
        }

        // -----------------------------
        // Create a new song
        // -----------------------------
        public async Task<Song> CreateAsync(Song song)
        {
            return await _songRepo.AddAsync(song);
        }

        // -----------------------------
        // Get song by ID
        // -----------------------------
        public async Task<Song> GetByIdAsync(Guid id)
        {
            return await _songRepo.GetByIdAsync(id);
        }

        // -----------------------------
        // Update song
        // -----------------------------
        public async Task UpdateAsync(Song song)
        {
            await _songRepo.UpdateAsync(song);
        }

        // -----------------------------
        // Delete song
        // -----------------------------
        public async Task DeleteAsync(Guid id)
        {
            await _songRepo.DeleteAsync(id);
        }
    }
}
