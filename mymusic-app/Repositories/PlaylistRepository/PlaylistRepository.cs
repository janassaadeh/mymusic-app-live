using Microsoft.EntityFrameworkCore;

using mymusic_app.Controllers.Data;
using mymusic_app.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mymusic_app.Repositories
{
    public class PlaylistRepository : IPlaylistRepository
    {
        private readonly AppDbContext _db;

        public PlaylistRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<Playlist> GetByIdAsync(Guid playlistId)
            => await _db.Playlists
                .Include(p => p.Songs)
                .ThenInclude(ps => ps.Song)
                .FirstOrDefaultAsync(p => p.Id == playlistId);

        public async Task<IEnumerable<Playlist>> GetUserPlaylistsAsync(Guid userId)
            => await _db.Playlists.Where(p => p.OwnerId == userId).ToListAsync();

        public async Task<IEnumerable<Playlist>> GetFollowedPlaylistsAsync(Guid userId)
            => await _db.PlaylistFollows
                .Where(f => f.UserId == userId)
                .Select(f => f.Playlist)
                .ToListAsync();

        public async Task CreatePlaylistAsync(Playlist playlist)
        {
            _db.Playlists.Add(playlist);
            await _db.SaveChangesAsync();
        }

        public async Task UpdatePlaylistAsync(Playlist playlist)
        {
            _db.Playlists.Update(playlist);
            await _db.SaveChangesAsync();
        }

        public async Task DeletePlaylistAsync(Guid playlistId)
        {
            var playlist = await GetByIdAsync(playlistId);
            if (playlist != null)
            {
                _db.Playlists.Remove(playlist);
                await _db.SaveChangesAsync();
            }
        }

        public async Task AddSongAsync(Guid playlistId, Guid songId, int position)
        {
            _db.PlaylistSongs.Add(new PlaylistSong
            {
                PlaylistId = playlistId,
                SongId = songId,
                Position = position
            });
            await _db.SaveChangesAsync();
        }

        public async Task RemoveSongAsync(Guid playlistId, Guid songId)
        {
            var ps = await _db.PlaylistSongs
                .FirstOrDefaultAsync(p => p.PlaylistId == playlistId && p.SongId == songId);

            if (ps != null)
            {
                _db.PlaylistSongs.Remove(ps);
                await _db.SaveChangesAsync();
            }
        }

        public async Task ReorderSongsAsync(Guid playlistId, List<Guid> newOrder)
        {
            var songs = await _db.PlaylistSongs
                .Where(ps => ps.PlaylistId == playlistId)
                .ToListAsync();

            for (int i = 0; i < newOrder.Count; i++)
            {
                var ps = songs.FirstOrDefault(s => s.SongId == newOrder[i]);
                if (ps != null) ps.Position = i + 1;
            }

            await _db.SaveChangesAsync();
        }

        public async Task FollowPlaylistAsync(Guid playlistId, Guid userId)
        {
            _db.PlaylistFollows.Add(new PlaylistFollow
            {
                PlaylistId = playlistId,
                UserId = userId
            });
            await _db.SaveChangesAsync();
        }

        public async Task UnfollowPlaylistAsync(Guid playlistId, Guid userId)
        {
            var pf = await _db.PlaylistFollows
                .FirstOrDefaultAsync(p => p.PlaylistId == playlistId && p.UserId == userId);

            if (pf != null)
            {
                _db.PlaylistFollows.Remove(pf);
                await _db.SaveChangesAsync();
            }
        }
        public async Task<IEnumerable<Playlist>> GetAllAsync()
        {
            return await _db.Playlists
                .Include(p => p.Songs)
                    .ThenInclude(ps => ps.Song)
                .ToListAsync();
        }

    }

}
