using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using mymusic_app.Models;
using mymusic_app.Services;
namespace mymusic_app.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("Admin/Genres")]
    public class GenresController : Controller
    {
        private readonly ISongService _songService;
        private readonly IGenreService _genreService;

        public GenresController(ISongService songService, IGenreService genreService)
        {
            _songService = songService;
            _genreService = genreService;
        }
        [HttpGet("TopSongsDashboard")]
        public async Task<IActionResult> TopSongsDashboard()
        {
            var genres = await _genreService.GetAllAsync();

            var topSongsByGenre = new List<GenreTopSongsViewModel>();

            foreach (var genre in genres)
            {
                var songs = await _songService.GetTopSongsByGenreAsync(genre.Id);
                topSongsByGenre.Add(new GenreTopSongsViewModel
                {
                    Genre = genre,
                    TopSongs = songs.Take(1).ToList() // or Take(5)
                });
            }

            return View(topSongsByGenre);
        }

    
}


    public class GenreTopSongsViewModel
    {
        public Genre Genre { get; set; } = null!;
        public List<Song> TopSongs { get; set; } = new();
    }
}


