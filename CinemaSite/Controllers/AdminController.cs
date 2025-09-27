using CinemaSite.Data;
using CinemaSite.Models;
using CinemaSite.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace CinemaSite.Controllers
{
    public class AdminController : Controller
    {
        private readonly CinemaDbContext _context;

        public AdminController(CinemaDbContext context)
        {
            _context = context;
        }

        public IActionResult RepertuarPanel()
        {
            var viewModel = new AdminViewModel
            {
                Movies = _context.Movie.OrderBy(m => m.movie_id).ToList(),
                Screenings = _context.Screening.OrderBy(s => s.movie_id).ToList(),
                Genres = _context.Genre.OrderBy(g => g.genre_id).ToList(),
                MovieGenreJoins = _context.MovieGenre.OrderBy(mg => mg.movie_id).ToList(),
                Halls = _context.Hall.ToList()
            };

            if (HttpContext.Session.GetInt32("ActiveUserID") == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var userAdminCheck = _context.UserAccount
                .FirstOrDefault(ua => ua.account_id == (int)HttpContext.Session.GetInt32("ActiveUserID")
                && ua.is_admin);

            if (userAdminCheck == null) {
                return RedirectToAction("Login", "Account");
            }

            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddMovieDB(NewMovieDTO NewMovie, List<string> genreCheckbox)
        {
            if (NewMovie == null) return RedirectToAction("RepertuarPanel");
            if (NewMovie.poster.Length > 10485760) return RedirectToAction("RepertuarPanel");
            //                           ^10MB

            MovieEntity newMovieEntity = new MovieEntity();

            newMovieEntity.title = NewMovie.title;
            newMovieEntity.summary = NewMovie.summary;
            newMovieEntity.trailer_link = NewMovie.trailer_link;
            newMovieEntity.for_kids = NewMovie.for_kids;

            Random rngesus = new Random();
            var newImageIndex = rngesus.Next(0, 99999999);
            var newImageName = "img" + newImageIndex + ".jpg";
            var newImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Content", newImageName);

            using (var iostream = new FileStream(newImagePath, FileMode.Create)) NewMovie.poster.CopyTo(iostream);

            newMovieEntity.poster_id = newImageIndex;

            _context.Movie.Add(newMovieEntity);
            _context.SaveChanges();

            for (var i = 0; i < genreCheckbox.Count(); i++)
            {
                MovieGenreJoinEntity newMovieGenreJoin = new MovieGenreJoinEntity();
                newMovieGenreJoin.movie_id = NewMovie.movie_id;
                newMovieGenreJoin.genre_id = Int32.Parse(genreCheckbox[i]);
                _context.Add(newMovieGenreJoin);
            }
            
            _context.SaveChanges();

            return RedirectToAction("RepertuarPanel");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditMovieDB(MovieDTO MovieEdit, List<string> genreCheckbox)
        {
            var movieEntityEdit = _context.Movie.FirstOrDefault(m => m.movie_id == MovieEdit.movie_id);

            if (movieEntityEdit == null) return RedirectToAction("RepertuarPanel");

            if (!MovieEdit.title.IsNullOrEmpty()) movieEntityEdit.title = MovieEdit.title;
            if (!MovieEdit.summary.IsNullOrEmpty()) movieEntityEdit.summary = MovieEdit.summary;
            if (!MovieEdit.trailer_link.IsNullOrEmpty()) movieEntityEdit.trailer_link = MovieEdit.trailer_link;
            movieEntityEdit.for_kids = MovieEdit.for_kids;

            //delete all pre-existing moviegenrejoins for that movie

            var deleteMovieGenres = _context.MovieGenre.Where(mg => mg.movie_id == MovieEdit.movie_id);
            _context.MovieGenre.RemoveRange(deleteMovieGenres);

            for (var i = 0; i < genreCheckbox.Count(); i++)
            {
                MovieGenreJoinEntity newMovieGenreJoin = new MovieGenreJoinEntity();
                newMovieGenreJoin.movie_id = MovieEdit.movie_id;
                newMovieGenreJoin.genre_id = Int32.Parse(genreCheckbox[i]);
                _context.Add(newMovieGenreJoin);
            }

            _context.SaveChanges();
            return RedirectToAction("RepertuarPanel");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditScreeningDB(List<int> movieId, List<int> screeningId, List<DateTime> screeningTime,
            List<int> hallId, List<string> isDubbing)
        {
            for (int i = 0; i < screeningId.Count(); i++)
            {
                var screeningEntityEdit = _context.Screening.FirstOrDefault(sc => sc.screening_id == screeningId[i]);

                if (movieId != null && screeningId != null && screeningEntityEdit != null)
                {
                    screeningEntityEdit.screening_time = screeningTime[i];
                    screeningEntityEdit.hall_id = hallId[i];
                    if (isDubbing[i] == "true")
                    {
                        screeningEntityEdit.dubbing = true;
                    } else
                    {
                        screeningEntityEdit.dubbing = false;
                    }
                }
            }

            _context.SaveChanges();
            return RedirectToAction("RepertuarPanel");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteMovie(int id)
        {
            var movieEntityEdit = _context.Movie.FirstOrDefault(m => m.movie_id == id);

            if (movieEntityEdit == null) return RedirectToAction("RepertuarPanel");

            _context.Movie.Remove(movieEntityEdit);

            _context.SaveChanges();
            return RedirectToAction("RepertuarPanel");
        }
    }
}
