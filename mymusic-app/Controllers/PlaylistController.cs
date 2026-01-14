using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using mymusic_app.Services;
using mymusic_app.Models;

namespace mymusic_app.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PlaylistsController : ControllerBase
    {
        private readonly PlaylistService _playlistService;

        public PlaylistsController(PlaylistService playlistService)
        {
            _playlistService = playlistService;
        }

        // GET api/playlists
        [HttpGet]
        public async Task<IActionResult> GetAllPlaylists()
        {
            var playlists = await _playlistService.GetAllPlaylistsAsync();
            return Ok(playlists);
        }

        // GET api/playlists/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> Details(Guid id)
        {
            var playlist = await _playlistService.GetByIdAsync(id);
            if (playlist == null)
                return NotFound();
            return Ok(playlist);
        }

        // POST api/playlists
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Playlist playlist)
        {
            await _playlistService.CreatePlaylistAsync(playlist);
            return CreatedAtAction(nameof(Details), new { id = playlist.Id }, playlist);
        }

        // DELETE api/playlists/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _playlistService.DeletePlaylistAsync(id);
            return NoContent();
        }

        // POST api/playlists/{playlistId}/songs
        [HttpPost("{playlistId}/songs")]
        public async Task<IActionResult> AddSong(Guid playlistId, [FromQuery] Guid songId, [FromQuery] int position)
        {
            await _playlistService.AddSongAsync(playlistId, songId, position);
            return NoContent();
        }

        // DELETE api/playlists/{playlistId}/songs/{songId}
        [HttpDelete("{playlistId}/songs/{songId}")]
        public async Task<IActionResult> RemoveSong(Guid playlistId, Guid songId)
        {
            await _playlistService.RemoveSongAsync(playlistId, songId);
            return NoContent();
        }

        // PUT api/playlists/{playlistId}/reorder
        [HttpPut("{playlistId}/reorder")]
        public async Task<IActionResult> Reorder(Guid playlistId, [FromBody] List<Guid> songIds)
        {
            await _playlistService.ReorderSongsAsync(playlistId, songIds);
            return NoContent();
        }
    }
}
