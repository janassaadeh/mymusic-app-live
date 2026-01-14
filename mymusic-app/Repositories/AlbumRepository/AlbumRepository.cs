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
    public class AlbumRepository : IAlbumRepository
    {
        private readonly AppDbContext _db;

        public AlbumRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<Album> GetByIdAsync(Guid albumId)
            => await _db.Albums
                .Include(a => a.Songs)
                .FirstOrDefaultAsync(a => a.Id == albumId);

        public async Task<IEnumerable<Song>> GetAlbumSongsAsync(Guid albumId)
            => await _db.Songs
                .Where(s => s.AlbumId == albumId)
                .ToListAsync();

        public async Task FollowAlbumAsync(Guid albumId, Guid userId)
        {
            _db.UserAlbumFollows.Add(new UserAlbumFollow
            {
                UserId = userId,
                AlbumId = albumId
            });

            await _db.SaveChangesAsync();
        }

        public async Task UnfollowAlbumAsync(Guid albumId, Guid userId)
        {
            var ua = await _db.UserAlbumFollows
                .FirstOrDefaultAsync(x => x.UserId == userId && x.AlbumId == albumId);

            if (ua != null)
            {
                _db.UserAlbumFollows.Remove(ua);
                await _db.SaveChangesAsync();
            }
        }

        /* ===== Missing implementations ===== */

        public async Task<Album> AddAsync(Album album)
        {
            _db.Albums.Add(album);
            await _db.SaveChangesAsync();
            return album;
        }

        public async Task<IEnumerable<Album>> GetAllAsync()
            => await _db.Albums
                .Include(a => a.Songs)
                .ToListAsync();

        public async Task UpdateAsync(Album album)
        {
            _db.Albums.Update(album);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid albumId)
        {
            var album = await _db.Albums.FindAsync(albumId);

            if (album != null)
            {
                _db.Albums.Remove(album);
                await _db.SaveChangesAsync();
            }
        }
    }
}
