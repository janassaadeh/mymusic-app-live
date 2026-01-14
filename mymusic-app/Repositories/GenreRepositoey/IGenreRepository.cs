using mymusic_app.Models;

namespace mymusic_app.Repositories
{

    public interface IGenreRepository
    {
        Task<Genre> AddAsync(Genre album);
        Task<IEnumerable<Genre>> GetAllAsync();
        Task<Genre> GetByIdAsync(Guid id);
        Task UpdateAsync(Genre genre);
        Task DeleteAsync(Guid id);
    }
}
