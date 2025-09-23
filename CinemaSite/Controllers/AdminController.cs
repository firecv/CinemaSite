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
                MovieGenreJoins = _context.MovieGenre.OrderBy(mg => mg.movie_id).ToList()
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
        public IActionResult EditMovieDB(MovieDTO MovieEdit)
        {//[Bind(Prefix = "MovieEdit")] 
            var movieEntityEdit = _context.Movie.FirstOrDefault(m => m.movie_id == MovieEdit.movie_id);

            if (movieEntityEdit == null) return RedirectToAction("RepertuarPanel");

            if (!MovieEdit.title.IsNullOrEmpty()) movieEntityEdit.title = MovieEdit.title;
            if (!MovieEdit.summary.IsNullOrEmpty()) movieEntityEdit.summary = MovieEdit.summary;
            if (!MovieEdit.trailer_link.IsNullOrEmpty()) movieEntityEdit.trailer_link = MovieEdit.trailer_link;
            movieEntityEdit.for_kids = MovieEdit.for_kids;

            _context.SaveChanges();
            return RedirectToAction("RepertuarPanel");
        }
    }
}
