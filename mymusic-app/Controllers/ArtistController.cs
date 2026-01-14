using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using mymusic_app.Services;

namespace mymusic_app.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ArtistController : ControllerBase
    {
        private readonly ArtistService _artistService;

        public ArtistController(ArtistService artistService)
        {
            _artistService = artistService;
        }

        // GET api/artist
        [HttpGet]
        public async Task<IActionResult> GetAllArtists()
        {
            var artists = await _artistService.GetAllAsync();
            return Ok(artists);
        }

        // GET api/artist/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> Details(Guid id)
        {
            var artist = await _artistService.GetArtistByIdAsync(id);
            if (artist == null)
                return NotFound();
            return Ok(artist);
        }

        // GET api/artist/{id}/topsongs
        [HttpGet("{id}/topsongs")]
        public async Task<IActionResult> TopSongs(Guid id)
        {
            var songs = await _artistService.GetTopSongsAsync(id);
            return Ok(songs);
        }

        // GET api/artist/{id}/albums
        [HttpGet("{id}/albums")]
        public async Task<IActionResult> Albums(Guid id)
        {
            var albums = await _artistService.GetAlbumsWithSongsAsync(id);
            return Ok(albums);
        }

        // GET api/artist/{id}/similar
        [HttpGet("{id}/similar")]
        public async Task<IActionResult> Similar(Guid id)
        {
            var artists = await _artistService.GetSimilarArtistsAsync(id);
            return Ok(artists);
        }
    }
}
