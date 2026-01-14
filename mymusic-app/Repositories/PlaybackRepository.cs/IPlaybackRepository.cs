using mymusic_app.Models;

namespace mymusic_app.Repositories
{
    public interface IPlaybackRepository
    {
        // ---------------- RECENTLY PLAYED ----------------
        Task<IEnumerable<Song>> GetRecentlyPlayedSongsAsync(Guid userId);
        Task<IEnumerable<(User User, Song Song)>> GetFollowedUsersRecentPlaysAsync(Guid userId);
        Task DeleteOldRecentlyPlayedAsync(Guid userId);

        // ---------------- SONG PLAYS ----------------
        Task RecordSongPlayAsync(Guid userId, Guid songId);

        // ---------------- LIKES ----------------
        Task LikeSongAsync(Guid userId, Guid songId);
        Task UnlikeSongAsync(Guid userId, Guid songId);

        // ---------------- FOLLOW USERS ----------------
        Task FollowUserAsync(Guid userId, Guid followUserId);
        Task UnfollowUserAsync(Guid userId, Guid followUserId);

        // ---------------- FOLLOW ARTISTS ----------------
        Task FollowArtistAsync(Guid userId, Guid artistId);
        Task UnfollowArtistAsync(Guid userId, Guid artistId);

        // ---------------- FOLLOW PLAYLISTS ----------------
        Task FollowPlaylistAsync(Guid userId, Guid playlistId);
        Task UnfollowPlaylistAsync(Guid userId, Guid playlistId);
    }
}
