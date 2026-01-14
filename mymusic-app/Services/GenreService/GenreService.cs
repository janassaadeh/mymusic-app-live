using mymusic_app.Models;
using mymusic_app.Repositories;

namespace mymusic_app.Services
{
    public class GenreService:IGenreService
    {
        private readonly IGenreRepository _repo;

        public GenreService(IGenreRepository repo)
        {
            _repo = repo;
        }

        // -----------------------------
        // Create genre
        // -----------------------------
        public async Task<Genre> CreateAsync(Genre genre)
        {
            return await _repo.AddAsync(genre);
        }

        // -----------------------------
        // Get all genres
        // -----------------------------
        public async Task<IEnumerable<Genre>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }

        // -----------------------------
        // Get genre by ID
        // -----------------------------
        public async Task<Genre> GetByIdAsync(Guid id)
        {
            return await _repo.GetByIdAsync(id);
        }

        // -----------------------------
        // Update genre
        // -----------------------------
        public async Task UpdateAsync(Genre genre)
        {
            await _repo.UpdateAsync(genre);
        }

        // -----------------------------
        // Delete genre
        // -----------------------------
        public async Task DeleteAsync(Guid id)
        {
            await _repo.DeleteAsync(id);
        }
    }
}
