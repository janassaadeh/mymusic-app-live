using Microsoft.EntityFrameworkCore;
using mymusic_app.Controllers.Data;
using mymusic_app.Models;
using mymusic_app.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mymusic_app.Repositories
{
    public class ArtistRepository : IArtistRepository
    {
        private readonly AppDbContext _db;

        public ArtistRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<Artist> AddAsync(Artist artist)
        {
            _db.Artists.Add(artist);
            await _db.SaveChangesAsync();
            return artist;
        }

        public async Task<IEnumerable<Artist>> GetAllAsync()
        {
            return await _db.Artists
                .Include(a => a.Albums)
                .Include(a => a.Genres)
                .ToListAsync();
        }

        public async Task<Artist> GetByIdAsync(Guid id)
        {
            return await _db.Artists
                .Include(a => a.Followers)
                .Include(a => a.Songs)
                    .ThenInclude(s => s.Plays)
                .Include(a => a.Albums)
                .Include(a => a.Genres)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<IEnumerable<Song>> GetTopSongsAsync(Guid artistId, int limit = 20)
        {
            return await _db.UserSongPlays
                .Where(p => p.Song.ArtistId == artistId)
                .GroupBy(p => p.Song)
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key)
                .Take(limit)
                .ToListAsync();
        }

        public async Task<IEnumerable<Album>> GetAlbumsWithSongsAsync(Guid artistId)
        {
            return await _db.Albums
                .Include(a => a.Songs)
                .Where(a => a.ArtistId == artistId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Artist>> GetSimilarArtistsAsync(Guid artistId, int limit = 7)
        {
            var genreIds = await _db.ArtistGenres
                .Where(ag => ag.ArtistId == artistId)
                .Select(ag => ag.GenreId)
                .ToListAsync();

            return await _db.ArtistGenres
                .Where(ag => genreIds.Contains(ag.GenreId) && ag.ArtistId != artistId)
                .Select(ag => ag.Artist)
                .Distinct()
                .Take(limit)
                .ToListAsync();
        }

        public async Task UpdateAsync(Artist artist)
        {
            _db.Artists.Update(artist);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid artistId)
        {
            var artist = await _db.Artists.FindAsync(artistId);

            if (artist != null)
            {
                _db.Artists.Remove(artist);
                await _db.SaveChangesAsync();
            }
        }
    }
}
