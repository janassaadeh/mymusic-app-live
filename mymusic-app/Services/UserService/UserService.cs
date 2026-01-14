using mymusic_app.Models;
using mymusic_app.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace mymusic_app.Services
{
    public class UserService:IUserService
    {
        private readonly IUserRepository _userRepo;
        private readonly IPlaybackRepository _playbackRepo;

        public UserService(IUserRepository userRepo, IPlaybackRepository playbackRepo)
        {
            _userRepo = userRepo;
            _playbackRepo = playbackRepo;
        }

        // ---------------- GET ALL / GET BY ID ----------------
        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _userRepo.GetAllAsync();
        }

        public async Task<User> GetByIdAsync(Guid userId)
        {
            return await _userRepo.GetByIdAsync(userId);
        }

        // ---------------- PROFILE ----------------
        public async Task<User> GetProfileAsync(Guid userId)
            => await _userRepo.GetByIdAsync(userId);

        public async Task UpdateProfileAsync(User user)
            => await _userRepo.UpdateAsync(user);

        // ---------------- FOLLOWERS / FOLLOWING ----------------
        public async Task<int> GetFollowersCountAsync(Guid userId)
            => await _userRepo.GetFollowersCountAsync(userId);

        public async Task<int> GetFollowingCountAsync(Guid userId)
            => await _userRepo.GetFollowingCountAsync(userId);

        public async Task<IEnumerable<User>> GetFollowersAsync(Guid userId)
            => await _userRepo.GetFollowersAsync(userId);

        public async Task<IEnumerable<User>> GetFollowingAsync(Guid userId)
            => await _userRepo.GetFollowingAsync(userId);

        // ---------------- TOP PLAYED SONGS ----------------
        public async Task<IEnumerable<Song>> GetTopPlayedSongsAsync(Guid userId, int limit = 20)
            => await _userRepo.GetTopPlayedSongsAsync(userId, limit);

        // ---------------- LIKES ----------------
        public async Task<int> GetLikesCountAsync(Guid userId)
            => await _userRepo.GetLikesCountAsync(userId);

        public async Task<IEnumerable<Song>> GetLikedSongsAsync(Guid userId)
            => await _userRepo.GetLikedSongsAsync(userId);

        public async Task LikeSongAsync(Guid userId, Guid songId)
            => await _userRepo.LikeSongAsync(userId, songId);

        public async Task UnlikeSongAsync(Guid userId, Guid songId)
            => await _userRepo.UnlikeSongAsync(userId, songId);

        // ---------------- FOLLOWED ARTISTS / ALBUMS ----------------
        public async Task<IEnumerable<Artist>> GetFollowedArtistsAsync(Guid userId)
            => await _userRepo.GetFollowedArtistsAsync(userId);

        public async Task FollowArtistAsync(Guid userId, Guid artistId)
            => await _userRepo.FollowArtistAsync(userId, artistId);

        public async Task UnfollowArtistAsync(Guid userId, Guid artistId)
            => await _userRepo.UnfollowArtistAsync(userId, artistId);

        public async Task<IEnumerable<Album>> GetFollowedAlbumsAsync(Guid userId)
            => await _userRepo.GetFollowedAlbumsAsync(userId);

        // ---------------- PLAYLISTS ----------------
        public async Task<IEnumerable<Playlist>> GetUserPlaylistsAsync(Guid userId)
            => await _userRepo.GetUserPlaylistsAsync(userId);

        public async Task<IEnumerable<Playlist>> GetFollowedPlaylistsAsync(Guid userId)
            => await _userRepo.GetFollowedPlaylistsAsync(userId);

        public async Task FollowPlaylistAsync(Guid userId, Guid playlistId)
            => await _userRepo.FollowPlaylistAsync(userId, playlistId);

        public async Task UnfollowPlaylistAsync(Guid userId, Guid playlistId)
            => await _userRepo.UnfollowPlaylistAsync(userId, playlistId);

        // ---------------- FOLLOW / UNFOLLOW USERS ----------------
        public async Task FollowUserAsync(Guid userId, Guid followUserId)
            => await _userRepo.FollowUserAsync(userId, followUserId);

        public async Task UnfollowUserAsync(Guid userId, Guid followUserId)
            => await _userRepo.UnfollowUserAsync(userId, followUserId);

        // ---------------- RECENTLY PLAYED ----------------
        public async Task<IEnumerable<Song>> GetRecentlyPlayedAsync(Guid userId)
        {
            var songs = await _playbackRepo.GetRecentlyPlayedSongsAsync(userId);
            await _playbackRepo.DeleteOldRecentlyPlayedAsync(userId);
            return songs;
        }

        public async Task PlaySongAsync(Guid userId, Guid songId)
            => await _playbackRepo.RecordSongPlayAsync(userId, songId);

        public async Task<IEnumerable<(User User, Song Song)>> GetFollowedUsersRecentPlaysAsync(Guid userId)
            => await _playbackRepo.GetFollowedUsersRecentPlaysAsync(userId);

        // ---------------- EDIT PROFILE ----------------
        public async Task EditProfileAsync(Guid userId, string FirstName, string LastName, string bio = null)
        {
            var user = await _userRepo.GetByIdAsync(userId);
            if (user == null)
                throw new Exception("User not found");

            user.FirstName = FirstName;
            user.LastName = LastName;

            if (bio != null)
                user.Bio = bio;

            await _userRepo.UpdateAsync(user);
        }
    }
}
