using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using mymusic_app.Controllers.Data;
using mymusic_app.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace mymusic_app.Repositories
{
    public class PlaybackRepository : IPlaybackRepository
    {
        private readonly string _connectionString;
        private readonly AppDbContext _db;

        public PlaybackRepository(IConfiguration config, AppDbContext db)
        {
            _connectionString = config.GetConnectionString("DefaultConnection");
            _db = db;
        }

        // ============================================================
        // RECENTLY PLAYED
        // ============================================================
        public async Task<IEnumerable<Song>> GetRecentlyPlayedSongsAsync(Guid userId)
        {
            return await _db.UserSongPlays
                .Where(p => p.UserId == userId)
                .OrderByDescending(p => p.PlayedAt)
                .Select(p => p.Song)
                .Include(s => s.Artist)
                .Include(s => s.Album)
                .Take(20)
                .ToListAsync();
        }


        public async Task DeleteOldRecentlyPlayedAsync(Guid userId)
        {
            var cutoff = DateTime.UtcNow.AddDays(-1);

            var oldRecords = await _db.UserSongPlays
                .Where(p => p.UserId == userId && p.PlayedAt < cutoff)
                .ToListAsync();

            _db.UserSongPlays.RemoveRange(oldRecords);
            await _db.SaveChangesAsync();
        }


        // ============================================================
        // FOLLOWED USERS RECENT PLAYS
        // ============================================================
        public async Task<IEnumerable<(User User, Song Song)>> GetFollowedUsersRecentPlaysAsync(Guid userId)
        {
            var result = new List<(User, Song)>();

            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            await using var cmd = new NpgsqlCommand(@"
                SELECT
                    u.Id,
                    u.FirstName,
                    u.LastName,
                    s.Id,
                    s.Title,
                    s.Duration,
                    a.Id AS ArtistId,
                    a.Name AS ArtistName,
                    al.Id AS AlbumId,
                    al.Title AS AlbumTitle,
                    al.CoverImageUrl
                FROM Followings f
                JOIN Users u ON f.FollowingId = u.Id
                JOIN ""UserSongPlays"" p ON p.UserId = u.Id
                JOIN Songs s ON s.Id = p.SongId
                LEFT JOIN Artists a ON s.ArtistId = a.Id
                LEFT JOIN Albums al ON s.AlbumId = al.Id
                WHERE f.UserId = @UserId
                ORDER BY p.PlayedAt DESC
                LIMIT 20
            ", conn);

            cmd.Parameters.AddWithValue("UserId", userId);

            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var user = new User
                {
                    Id = reader.GetGuid(0),
                    FirstName = reader.GetString(1),
                    LastName = reader.GetString(2)
                };

                var song = new Song
                {
                    Id = reader.GetGuid(3),
                    Title = reader.GetString(4),
                    Duration = reader.GetTimeSpan(5),
                    Artist = reader.IsDBNull(6) ? null : new Artist
                    {
                        Id = reader.GetGuid(6),
                        Name = reader.GetString(7)
                    },
                    Album = reader.IsDBNull(8) ? null : new Album
                    {
                        Id = reader.GetGuid(8),
                        Title = reader.GetString(9),
                        CoverImageUrl = reader.IsDBNull(10) ? null : reader.GetString(10)
                    }
                };

                result.Add((user, song));
            }

            return result;
        }

        // ============================================================
        // SONG PLAYS
        // ============================================================
        public async Task RecordSongPlayAsync(Guid userId, Guid songId)
        {
            var songExists = await _db.Songs
                .AnyAsync(s => s.Id == songId);

            if (!songExists)
                throw new KeyNotFoundException($"Song {songId} does not exist.");

            var play = new UserSongPlay
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                SongId = songId,
                PlayedAt = DateTime.UtcNow
            };

            _db.UserSongPlays.Add(play);

            await _db.SaveChangesAsync();
        }


        // ============================================================
        // LIKES
        // ============================================================
        public async Task LikeSongAsync(Guid userId, Guid songId)
        {
            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            await using var cmd = new NpgsqlCommand(@"
                INSERT INTO UserSongLikes (Id, UserId, SongId)
                SELECT gen_random_uuid(), @UserId, @SongId
                WHERE NOT EXISTS (
                    SELECT 1 FROM UserSongLikes WHERE UserId = @UserId AND SongId = @SongId
                )
            ", conn);

            cmd.Parameters.AddWithValue("UserId", userId);
            cmd.Parameters.AddWithValue("SongId", songId);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task UnlikeSongAsync(Guid userId, Guid songId)
        {
            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            await using var cmd = new NpgsqlCommand(@"
                DELETE FROM UserSongLikes
                WHERE UserId = @UserId AND SongId = @SongId
            ", conn);

            cmd.Parameters.AddWithValue("UserId", userId);
            cmd.Parameters.AddWithValue("SongId", songId);

            await cmd.ExecuteNonQueryAsync();
        }

        // ============================================================
        // FOLLOW USERS
        // ============================================================
        public async Task FollowUserAsync(Guid userId, Guid followUserId)
        {
            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            await using var cmd = new NpgsqlCommand(@"
                INSERT INTO Followings (Id, UserId, FollowingId)
                SELECT gen_random_uuid(), @UserId, @FollowId
                WHERE NOT EXISTS (
                    SELECT 1 FROM Followings WHERE UserId = @UserId AND FollowingId = @FollowId
                )
            ", conn);

            cmd.Parameters.AddWithValue("UserId", userId);
            cmd.Parameters.AddWithValue("FollowId", followUserId);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task UnfollowUserAsync(Guid userId, Guid followUserId)
        {
            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            await using var cmd = new NpgsqlCommand(@"
                DELETE FROM Followings
                WHERE UserId = @UserId AND FollowingId = @FollowId
            ", conn);

            cmd.Parameters.AddWithValue("UserId", userId);
            cmd.Parameters.AddWithValue("FollowId", followUserId);

            await cmd.ExecuteNonQueryAsync();
        }

        // ============================================================
        // FOLLOW ARTISTS
        // ============================================================
        public async Task FollowArtistAsync(Guid userId, Guid artistId)
        {
            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            await using var cmd = new NpgsqlCommand(@"
                INSERT INTO UserArtistFollows (Id, UserId, ArtistId)
                SELECT gen_random_uuid(), @UserId, @ArtistId
                WHERE NOT EXISTS (
                    SELECT 1 FROM UserArtistFollows WHERE UserId = @UserId AND ArtistId = @ArtistId
                )
            ", conn);

            cmd.Parameters.AddWithValue("UserId", userId);
            cmd.Parameters.AddWithValue("ArtistId", artistId);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task UnfollowArtistAsync(Guid userId, Guid artistId)
        {
            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            await using var cmd = new NpgsqlCommand(@"
                DELETE FROM UserArtistFollows
                WHERE UserId = @UserId AND ArtistId = @ArtistId
            ", conn);

            cmd.Parameters.AddWithValue("UserId", userId);
            cmd.Parameters.AddWithValue("ArtistId", artistId);

            await cmd.ExecuteNonQueryAsync();
        }

        // ============================================================
        // FOLLOW PLAYLISTS
        // ============================================================
        public async Task FollowPlaylistAsync(Guid userId, Guid playlistId)
        {
            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            await using var cmd = new NpgsqlCommand(@"
                INSERT INTO UserPlaylistFollows (Id, UserId, PlaylistId)
                SELECT gen_random_uuid(), @UserId, @PlaylistId
                WHERE NOT EXISTS (
                    SELECT 1 FROM UserPlaylistFollows WHERE UserId = @UserId AND PlaylistId = @PlaylistId
                )
            ", conn);

            cmd.Parameters.AddWithValue("UserId", userId);
            cmd.Parameters.AddWithValue("PlaylistId", playlistId);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task UnfollowPlaylistAsync(Guid userId, Guid playlistId)
        {
            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            await using var cmd = new NpgsqlCommand(@"
                DELETE FROM UserPlaylistFollows
                WHERE UserId = @UserId AND PlaylistId = @PlaylistId
            ", conn);

            cmd.Parameters.AddWithValue("UserId", userId);
            cmd.Parameters.AddWithValue("PlaylistId", playlistId);

            await cmd.ExecuteNonQueryAsync();
        }
    }
}
