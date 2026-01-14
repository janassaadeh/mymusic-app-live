using mymusic_app.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace mymusic_app.Services
{
    public interface IGenreService
    {
        // -----------------------------
        // Create genre
        // -----------------------------
        Task<Genre> CreateAsync(Genre genre);

        // -----------------------------
        // Get all genres
        // -----------------------------
        Task<IEnumerable<Genre>> GetAllAsync();

        // -----------------------------
        // Get genre by ID
        // -----------------------------
        Task<Genre> GetByIdAsync(Guid id);

        // -----------------------------
        // Update genre
        // -----------------------------
        Task UpdateAsync(Genre genre);

        // -----------------------------
        // Delete genre
        // -----------------------------
        Task DeleteAsync(Guid id);
    }
}
