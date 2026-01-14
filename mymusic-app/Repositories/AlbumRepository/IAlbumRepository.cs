using mymusic_app.Models;

namespace mymusic_app.Repositories
{
    public interface IAlbumRepository
    {
        Task<Album> GetByIdAsync(Guid albumId);
        Task<IEnumerable<Song>> GetAlbumSongsAsync(Guid albumId);
        Task FollowAlbumAsync(Guid albumId, Guid userId);
        Task UnfollowAlbumAsync(Guid albumId, Guid userId);
        Task<Album> AddAsync(Album album);
        Task<IEnumerable<Album>> GetAllAsync();
        Task UpdateAsync(Album album);
        Task DeleteAsync(Guid albumId);
    }
}
