using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using mymusic_app.Models;
using mymusic_app.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mymusic_app.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IAlbumService _albumService;
        private readonly IArtistService _artistService;
        private readonly ISongService _songService;
        private readonly IGenreService _genreService;

        public AdminController(
            IAlbumService albumService,
            IArtistService artistService,
            ISongService songService,
            IGenreService genreService)
        {
            _albumService = albumService;
            _artistService = artistService;
            _songService = songService;
            _genreService = genreService;
        }

        // GET: /Admin/
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ViewBag.Artists = await _artistService.GetAllAsync();
            ViewBag.Albums = await _albumService.GetAllAsync();
            ViewBag.Genres = await _genreService.GetAllAsync();
            ViewBag.Songs = await _songService.GetAllAsync();

            return View();
        }

        // POST: /Admin/CreateAlbum
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAlbum(
            string Title,
            string CoverImageUrl,
            DateTime ReleaseDate,
            Guid ArtistId)
        {
            var album = new Album
            {
                Title = Title,
                CoverImageUrl = CoverImageUrl,
                ReleaseDate = ReleaseDate,
                ArtistId = ArtistId
            };

            await _albumService.CreateAsync(album);
            return RedirectToAction("Index");
        }

        // POST: /Admin/CreateArtist
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateArtist(
            string Name,
            string ImageUrl,
            List<Guid> GenreIds)
        {
            var artist = new Artist
            {
                Name = Name,
                ImageUrl = ImageUrl,
                Genres = new List<ArtistGenre>()
            };

            if (GenreIds != null && GenreIds.Any())
            {
                foreach (var genreId in GenreIds)
                {
                    artist.Genres.Add(new ArtistGenre { GenreId = genreId });
                }
            }

            await _artistService.CreateAsync(artist);
            return RedirectToAction("Index");
        }

        // POST: /Admin/CreateSong
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSong(
            string Title,
            string Duration,
            string DeezerTrackId,
            Guid ArtistId,
            Guid AlbumId)
        {
            var artist = await _artistService.GetArtistByIdAsync(ArtistId);

            var song = new Song
            {
                Title = Title,
                Duration = TimeSpan.Parse(Duration),
                DeezerTrackId = DeezerTrackId,
                ArtistId = ArtistId,
                AlbumId = AlbumId,
                Genres = new List<SongGenre>()
            };

            if (artist.Genres != null && artist.Genres.Any())
            {
                foreach (var ag in artist.Genres)
                {
                    song.Genres.Add(new SongGenre { GenreId = ag.GenreId });
                }
            }

            await _songService.CreateAsync(song);
            return RedirectToAction("Index");
        }

        // POST: /Admin/CreateGenre
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateGenre(string Name)
        {
            var genre = new Genre { Name = Name };
            await _genreService.CreateAsync(genre);
            return RedirectToAction("Index");
        }
        // DELETE GENRE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteGenre(Guid id)
        {
            var genre = await _genreService.GetByIdAsync(id);
            if (genre != null)
            {
                await _genreService.DeleteAsync(id);
            }
            return RedirectToAction("Index");
        }

        // DELETE ARTIST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteArtist(Guid id)
        {
            var artist = await _artistService.GetArtistByIdAsync(id);
            if (artist != null)
            {
                await _artistService.DeleteAsync(id);
            }
            return RedirectToAction("Index");
        }

        // DELETE ALBUM
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAlbum(Guid id)
        {
            var album = await _albumService.GetByIdAsync(id);
            if (album != null)
            {
                await _albumService.DeleteAsync(id);
            }
            return RedirectToAction("Index");
        }

        // DELETE SONG
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteSong(Guid id)
        {
            var song = await _songService.GetByIdAsync(id);
            if (song != null)
            {
                await _songService.DeleteAsync(id);
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateGenre(Guid id, string Name)
        {
            var genre = await _genreService.GetByIdAsync(id);
            if (genre != null)
            {
                genre.Name = Name;
                await _genreService.UpdateAsync(genre);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateArtist(Guid id, string Name, string ImageUrl)
        {
            var artist = await _artistService.GetArtistByIdAsync(id);
            if (artist != null)
            {
                artist.Name = Name;
                artist.ImageUrl = ImageUrl;
                await _artistService.UpdateAsync(artist);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateAlbum(Guid id, string Title, string CoverImageUrl, DateTime ReleaseDate)
        {
            var album = await _albumService.GetByIdAsync(id);
            if (album != null)
            {
                album.Title = Title;
                album.CoverImageUrl = CoverImageUrl;
                album.ReleaseDate = ReleaseDate;
                await _albumService.UpdateAsync(album);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateSong(Guid id, string Title, string Duration, string DeezerTrackId)
        {
            var song = await _songService.GetByIdAsync(id);
            if (song != null)
            {
                song.Title = Title;
                song.Duration = TimeSpan.Parse(Duration);
                song.DeezerTrackId = DeezerTrackId;
                await _songService.UpdateAsync(song);
            }
            return RedirectToAction("Index");
        }
    }

}


