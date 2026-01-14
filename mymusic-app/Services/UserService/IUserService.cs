using mymusic_app.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace mymusic_app.Services
{
    public interface IUserService
    {
        // ---------------- GET ALL / GET BY ID ----------------
        Task<IEnumerable<User>> GetAllAsync();
        Task<User> GetByIdAsync(Guid userId);

        // ---------------- PROFILE ----------------
        Task<User> GetProfileAsync(Guid userId);
        Task UpdateProfileAsync(User user);
        Task EditProfileAsync(Guid userId, string firstName, string lastName, string bio = null);

        // ---------------- FOLLOWERS / FOLLOWING ----------------
        Task<int> GetFollowersCountAsync(Guid userId);
        Task<int> GetFollowingCountAsync(Guid userId);
        Task<IEnumerable<User>> GetFollowersAsync(Guid userId);
        Task<IEnumerable<User>> GetFollowingAsync(Guid userId);

        // ---------------- TOP PLAYED SONGS ----------------
        Task<IEnumerable<Song>> GetTopPlayedSongsAsync(Guid userId, int limit = 20);

        // ---------------- LIKES ----------------
        Task<int> GetLikesCountAsync(Guid userId);
        Task<IEnumerable<Song>> GetLikedSongsAsync(Guid userId);
        Task LikeSongAsync(Guid userId, Guid songId);
        Task UnlikeSongAsync(Guid userId, Guid songId);

        // ---------------- FOLLOWED ARTISTS / ALBUMS ----------------
        Task<IEnumerable<Artist>> GetFollowedArtistsAsync(Guid userId);
        Task FollowArtistAsync(Guid userId, Guid artistId);
        Task UnfollowArtistAsync(Guid userId, Guid artistId);
        Task<IEnumerable<Album>> GetFollowedAlbumsAsync(Guid userId);

        // ---------------- PLAYLISTS ----------------
        Task<IEnumerable<Playlist>> GetUserPlaylistsAsync(Guid userId);
        Task<IEnumerable<Playlist>> GetFollowedPlaylistsAsync(Guid userId);
        Task FollowPlaylistAsync(Guid userId, Guid playlistId);
        Task UnfollowPlaylistAsync(Guid userId, Guid playlistId);

        // ---------------- FOLLOW / UNFOLLOW USERS ----------------
        Task FollowUserAsync(Guid userId, Guid followUserId);
        Task UnfollowUserAsync(Guid userId, Guid followUserId);

        // ---------------- RECENTLY PLAYED ----------------
        Task<IEnumerable<Song>> GetRecentlyPlayedAsync(Guid userId);
        Task PlaySongAsync(Guid userId, Guid songId);
        Task<IEnumerable<(User User, Song Song)>> GetFollowedUsersRecentPlaysAsync(Guid userId);
    }
}
