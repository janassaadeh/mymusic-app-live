using mymusic_app.Models;

namespace mymusic_app.Repositories
{
    public interface IArtistRepository
    {
        Task<Artist> GetByIdAsync(Guid id);
        Task<IEnumerable<Song>> GetTopSongsAsync(Guid artistId, int limit = 20);
        Task<IEnumerable<Album>> GetAlbumsWithSongsAsync(Guid artistId);
        Task<IEnumerable<Artist>> GetSimilarArtistsAsync(Guid artistId, int limit = 7);
        Task<Artist> AddAsync(Artist artist);
        Task<IEnumerable<Artist>> GetAllAsync();
        Task UpdateAsync(Artist artist);
        Task DeleteAsync(Guid artistId);
    }

}
