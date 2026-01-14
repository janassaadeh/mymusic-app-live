using mymusic_app.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace mymusic_app.Services
{
    public interface ISongService
    {
        // -----------------------------
        // Get top songs by genre
        // -----------------------------
        Task<IEnumerable<Song>> GetTopSongsByGenreAsync(Guid genreId, int limit = 20);

        // -----------------------------
        // Get all songs
        // -----------------------------
        Task<IEnumerable<Song>> GetAllAsync();

        // -----------------------------
        // Create a new song
        // -----------------------------
        Task<Song> CreateAsync(Song song);

        // -----------------------------
        // Get song by ID
        // -----------------------------
        Task<Song> GetByIdAsync(Guid id);

        // -----------------------------
        // Update song
        // -----------------------------
        Task UpdateAsync(Song song);

        // -----------------------------
        // Delete song
        // -----------------------------
        Task DeleteAsync(Guid id);
    }
}
