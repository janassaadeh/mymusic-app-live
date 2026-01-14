
using mymusic_app.Models;
using mymusic_app.Repositories;


namespace mymusic_app.Services
{
    public class ArtistService:IArtistService
    {
        private readonly IArtistRepository _artistRepo;

        public ArtistService(IArtistRepository artistRepo)
        {
            _artistRepo = artistRepo;
        }

        // -----------------------------
        // Get artist by Id with counts
        // -----------------------------
        public async Task<Artist> GetArtistByIdAsync(Guid artistId)
        {
            var artist = await _artistRepo.GetByIdAsync(artistId);
            if (artist == null)
                return null;

            return artist;
        }

        // -----------------------------
        // Get top songs
        // -----------------------------
        public async Task<IEnumerable<Song>> GetTopSongsAsync(Guid artistId, int limit = 20)
        {
            return await _artistRepo.GetTopSongsAsync(artistId, limit);
        }

        // -----------------------------
        // Get all artists
        // -----------------------------
        public async Task<IEnumerable<Artist>> GetAllAsync()
        {
            return await _artistRepo.GetAllAsync();
        }

        // -----------------------------
        // Get albums with songs
        // -----------------------------
        public async Task<IEnumerable<Album>> GetAlbumsWithSongsAsync(Guid artistId)
        {
            return await _artistRepo.GetAlbumsWithSongsAsync(artistId);
        }

        // -----------------------------
        // Get similar artists
        // -----------------------------
        public async Task<IEnumerable<Artist>> GetSimilarArtistsAsync(Guid artistId, int limit = 7)
        {
            return await _artistRepo.GetSimilarArtistsAsync(artistId, limit);
        }

        // -----------------------------
        // Create artist
        // -----------------------------
        public async Task<Artist> CreateAsync(Artist artist)
        {
            return await _artistRepo.AddAsync(artist);
        }

        // -----------------------------
        // Update artist
        // -----------------------------
        public async Task UpdateAsync(Artist artist)
        {
            await _artistRepo.UpdateAsync(artist);
        }

        // -----------------------------
        // Delete artist
        // -----------------------------
        public async Task DeleteAsync(Guid artistId)
        {
            await _artistRepo.DeleteAsync(artistId);
        }
    }
}
