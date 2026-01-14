using mymusic_app.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace mymusic_app.Services
{
    public interface IAlbumService
    {
        // -----------------------------
        // Get by ID
        // -----------------------------
        Task<Album> GetByIdAsync(Guid albumId);

        // -----------------------------
        // Get all albums
        // -----------------------------
        Task<IEnumerable<Album>> GetAllAsync();

        // -----------------------------
        // Get songs of album
        // -----------------------------
        Task<IEnumerable<Song>> GetAlbumSongsAsync(Guid albumId);

        // -----------------------------
        // Follow / Unfollow
        // -----------------------------
        Task FollowAlbumAsync(Guid albumId, Guid userId);
        Task UnfollowAlbumAsync(Guid albumId, Guid userId);

        // -----------------------------
        // Create album
        // -----------------------------
        Task<Album> CreateAsync(Album album);

        // -----------------------------
        // Update album
        // -----------------------------
        Task UpdateAsync(Album album);

        // -----------------------------
        // Delete album
        // -----------------------------
        Task DeleteAsync(Guid albumId);
    }
}
