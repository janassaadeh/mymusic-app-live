using Microsoft.Data.SqlClient;
using mymusic_app.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace mymusic_app.Repositories
{
    public class PlaybackRepository : IPlaybackRepository
    {
        private readonly string _connectionString;

        public PlaybackRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection");
        }

        // ---------------- RECENTLY PLAYED ----------------

        public async Task DeleteOldRecentlyPlayedAsync(Guid userId)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new SqlCommand(
                @"DELETE FROM Playbacks
                  WHERE UserId = @UserId AND PlayedAt < DATEADD(day, -1, GETDATE())", conn);

            cmd.Parameters.AddWithValue("@UserId", userId);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<IEnumerable<Song>> GetRecentlyPlayedSongsAsync(Guid userId)
        {
            var list = new List<Song>();
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new SqlCommand(
                @"SELECT TOP 20 s.Id, s.Title, s.ArtistId
                  FROM Playbacks p
                  JOIN Songs s ON p.SongId = s.Id
                  WHERE p.UserId = @UserId
                  ORDER BY p.PlayedAt DESC", conn);

            cmd.Parameters.AddWithValue("@UserId", userId);

            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(new Song
                {
                    Id = reader.GetGuid(0),
                    Title = reader.GetString(1),
                    ArtistId = reader.GetGuid(2)
                });
            }

            return list;
        }

        public async Task<IEnumerable<(User User, Song Song)>> GetFollowedUsersRecentPlaysAsync(Guid userId)
        {
            var result = new List<(User, Song)>();
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new SqlCommand(
                @"SELECT u.Id, u.FirstName, u.LastName, s.Id, s.Title, s.ArtistId
                  FROM Followings f
                  JOIN Users u ON f.FollowingId = u.Id
                  JOIN Playbacks p ON p.UserId = u.Id
                  JOIN Songs s ON s.Id = p.SongId
                  WHERE f.UserId = @UserId
                  ORDER BY p.PlayedAt DESC", conn);

            cmd.Parameters.AddWithValue("@UserId", userId);

            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                result.Add((
                    new User
                    {
                        Id = reader.GetGuid(0),
                        FirstName = reader.GetString(1),
                        LastName = reader.GetString(2)
                    },
                    new Song
                    {
                        Id = reader.GetGuid(3),
                        Title = reader.GetString(4),
                        ArtistId = reader.GetGuid(5)
                    }
                ));
            }

            return result;
        }

        // ---------------- SONG PLAYS ----------------

        public async Task RecordSongPlayAsync(Guid userId, Guid songId)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new SqlCommand(
                @"INSERT INTO Playbacks (Id, UserId, SongId, PlayedAt)
                  VALUES (NEWID(), @UserId, @SongId, GETDATE())", conn);

            cmd.Parameters.AddWithValue("@UserId", userId);
            cmd.Parameters.AddWithValue("@SongId", songId);

            await cmd.ExecuteNonQueryAsync();
        }

        // ---------------- LIKES ----------------

        public async Task LikeSongAsync(Guid userId, Guid songId)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new SqlCommand(
                @"IF NOT EXISTS (
                      SELECT 1 FROM UserSongLikes 
                      WHERE UserId = @UserId AND SongId = @SongId
                  )
                  INSERT INTO UserSongLikes (UserId, SongId)
                  VALUES (@UserId, @SongId)", conn);

            cmd.Parameters.AddWithValue("@UserId", userId);
            cmd.Parameters.AddWithValue("@SongId", songId);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task UnlikeSongAsync(Guid userId, Guid songId)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new SqlCommand(
                @"DELETE FROM UserSongLikes
                  WHERE UserId = @UserId AND SongId = @SongId", conn);

            cmd.Parameters.AddWithValue("@UserId", userId);
            cmd.Parameters.AddWithValue("@SongId", songId);

            await cmd.ExecuteNonQueryAsync();
        }

        // ---------------- FOLLOW USERS ----------------

        public async Task FollowUserAsync(Guid userId, Guid followUserId)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new SqlCommand(
                @"IF NOT EXISTS (
                      SELECT 1 FROM Followings
                      WHERE UserId = @UserId AND FollowingId = @FollowId
                  )
                  INSERT INTO Followings (UserId, FollowingId)
                  VALUES (@UserId, @FollowId)", conn);

            cmd.Parameters.AddWithValue("@UserId", userId);
            cmd.Parameters.AddWithValue("@FollowId", followUserId);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task UnfollowUserAsync(Guid userId, Guid followUserId)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new SqlCommand(
                @"DELETE FROM Followings
                  WHERE UserId = @UserId AND FollowingId = @FollowId", conn);

            cmd.Parameters.AddWithValue("@UserId", userId);
            cmd.Parameters.AddWithValue("@FollowId", followUserId);

            await cmd.ExecuteNonQueryAsync();
        }

        // ---------------- FOLLOW ARTISTS ----------------

        public async Task FollowArtistAsync(Guid userId, Guid artistId)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new SqlCommand(
                @"IF NOT EXISTS (
                      SELECT 1 FROM UserArtistFollows
                      WHERE UserId = @UserId AND ArtistId = @ArtistId
                  )
                  INSERT INTO UserArtistFollows (UserId, ArtistId)
                  VALUES (@UserId, @ArtistId)", conn);

            cmd.Parameters.AddWithValue("@UserId", userId);
            cmd.Parameters.AddWithValue("@ArtistId", artistId);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task UnfollowArtistAsync(Guid userId, Guid artistId)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new SqlCommand(
                @"DELETE FROM UserArtistFollows
                  WHERE UserId = @UserId AND ArtistId = @ArtistId", conn);

            cmd.Parameters.AddWithValue("@UserId", userId);
            cmd.Parameters.AddWithValue("@ArtistId", artistId);

            await cmd.ExecuteNonQueryAsync();
        }

        // ---------------- FOLLOW PLAYLISTS ----------------

        public async Task FollowPlaylistAsync(Guid userId, Guid playlistId)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new SqlCommand(
                @"IF NOT EXISTS (
                      SELECT 1 FROM UserPlaylistFollows
                      WHERE UserId = @UserId AND PlaylistId = @PlaylistId
                  )
                  INSERT INTO UserPlaylistFollows (UserId, PlaylistId)
                  VALUES (@UserId, @PlaylistId)", conn);

            cmd.Parameters.AddWithValue("@UserId", userId);
            cmd.Parameters.AddWithValue("@PlaylistId", playlistId);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task UnfollowPlaylistAsync(Guid userId, Guid playlistId)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new SqlCommand(
                @"DELETE FROM UserPlaylistFollows
                  WHERE UserId = @UserId AND PlaylistId = @PlaylistId", conn);

            cmd.Parameters.AddWithValue("@UserId", userId);
            cmd.Parameters.AddWithValue("@PlaylistId", playlistId);

            await cmd.ExecuteNonQueryAsync();
        }
    }
}
