using Microsoft.EntityFrameworkCore;
using mymusic_app.Controllers.Data;
using mymusic_app.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace mymusic_app.Repositories
{
    public class GenreRepository : IGenreRepository
    {
        private readonly AppDbContext _db;

        public GenreRepository(AppDbContext db)
        {
            _db = db;
        }

        // ---------------- ADD ----------------
        public async Task<Genre> AddAsync(Genre genre)
        {
            _db.Genres.Add(genre);
            await _db.SaveChangesAsync();
            return genre;
        }

        // ---------------- GET ALL ----------------
        public async Task<IEnumerable<Genre>> GetAllAsync()
        {
            return await _db.Genres.ToListAsync();
        }

        // ---------------- GET BY ID ----------------
        public async Task<Genre> GetByIdAsync(Guid id)
        {
            return await _db.Genres.FirstOrDefaultAsync(g => g.Id == id);
        }

        // ---------------- UPDATE ----------------
        public async Task UpdateAsync(Genre genre)
        {
            _db.Genres.Update(genre);
            await _db.SaveChangesAsync();
        }

        // ---------------- DELETE ----------------
        public async Task DeleteAsync(Guid id)
        {
            var genre = await _db.Genres.FindAsync(id);
            if (genre != null)
            {
                _db.Genres.Remove(genre);
                await _db.SaveChangesAsync();
            }
        }
    }
}

