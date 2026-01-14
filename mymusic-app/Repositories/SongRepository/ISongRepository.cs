using mymusic_app.Models;

namespace mymusic_app.Repositories
{
    public interface ISongRepository
    {
        Task<IEnumerable<Song>> GetTopSongsByGenreAsync(Guid genreId, int limit = 20);
        Task<Song> AddAsync(Song song);
        Task<IEnumerable<Song>> GetAllAsync();
        Task UpdateAsync(Song song);
        Task DeleteAsync(Guid id);
        Task<Song> GetByIdAsync(Guid id);


    }
}
