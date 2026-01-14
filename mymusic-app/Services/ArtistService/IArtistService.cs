using mymusic_app.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace mymusic_app.Services
{
    public interface IArtistService
    {
        // -----------------------------
        // Get artist by Id with counts
        // -----------------------------
        Task<Artist> GetArtistByIdAsync(Guid artistId);

        // -----------------------------
        // Get top songs
        // -----------------------------
        Task<IEnumerable<Song>> GetTopSongsAsync(Guid artistId, int limit = 20);

        // -----------------------------
        // Get all artists
        // -----------------------------
        Task<IEnumerable<Artist>> GetAllAsync();

        // -----------------------------
        // Get albums with songs
        // -----------------------------
        Task<IEnumerable<Album>> GetAlbumsWithSongsAsync(Guid artistId);

        // -----------------------------
        // Get similar artists
        // -----------------------------
        Task<IEnumerable<Artist>> GetSimilarArtistsAsync(Guid artistId, int limit = 7);

        // -----------------------------
        // Create artist
        // -----------------------------
        Task<Artist> CreateAsync(Artist artist);

        // -----------------------------
        // Update artist
        // -----------------------------
        Task UpdateAsync(Artist artist);

        // -----------------------------
        // Delete artist
        // -----------------------------
        Task DeleteAsync(Guid artistId);
    }
}
