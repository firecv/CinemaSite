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

        public void CleanOldData()
        {
            _context.Screening.Where(s => s.screening_time < DateTime.Now).ExecuteDelete();
            _context.Ticket.Where(t => t.hold_until < DateTime.Now && t.ticket_status != 2).ExecuteDelete();
        }

        public IActionResult RepertuarPanel()
        {
            CleanOldData();

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
                return RedirectToAction("Logowanie", "Account");
            }

            var userAdminCheck = _context.UserAccount
                .FirstOrDefault(ua => ua.account_id == (int)HttpContext.Session.GetInt32("ActiveUserID")
                && ua.is_admin);

            if (userAdminCheck == null) {
                return RedirectToAction("Logowanie", "Account");
            }

            return View(viewModel);
        }

        public IActionResult ZgloszeniePanel()
        {
            CleanOldData();

            var viewModel = new AdminViewModel
            {
                Articles = _context.Article.OrderByDescending(a => a.article_id).ToList()
            };

            if (HttpContext.Session.GetInt32("ActiveUserID") == null)
            {
                return RedirectToAction("Logowanie", "Account");
            }

            var userAdminCheck = _context.UserAccount
                .FirstOrDefault(ua => ua.account_id == (int)HttpContext.Session.GetInt32("ActiveUserID")
                && ua.is_admin);

            if (userAdminCheck == null)
            {
                return RedirectToAction("Logowanie", "Account");
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
                newMovieGenreJoin.movie_id = newMovieEntity.movie_id;
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
            var toBeDeletedFlag = new DateTime(1912, 12, 12, 12, 12, 0);

            for (int i = 0; i < screeningId.Count(); i++)
            {
                if (screeningId[i] == 0)
                {
                    ScreeningEntity newScreening = new ScreeningEntity();
                    newScreening.movie_id = movieId[i];
                    newScreening.screening_time = screeningTime[i];
                    newScreening.hall_id = hallId[i];
                    newScreening.dubbing = (isDubbing[i] == "true");

                    _context.Screening.Add(newScreening);
                }
                else
                {
                    var screeningEntityEdit = _context.Screening.FirstOrDefault(sc => sc.screening_id == screeningId[i]);

                    if (screeningTime[i] == toBeDeletedFlag)
                    {
                        _context.Screening.Remove(screeningEntityEdit);
                    } else
                    {
                        if (movieId != null && screeningId != null && screeningEntityEdit != null)
                        {
                            screeningEntityEdit.screening_time = screeningTime[i];
                            screeningEntityEdit.hall_id = hallId[i];
                            screeningEntityEdit.dubbing = (isDubbing[i] == "true");
                        }
                    }
                }
            }

            _context.SaveChanges();
            return RedirectToAction("RepertuarPanel");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteMovie(int id, [FromServices] IWebHostEnvironment www)
        {
            var movieEntityEdit = _context.Movie.FirstOrDefault(m => m.movie_id == id);

            if (movieEntityEdit == null) return RedirectToAction("RepertuarPanel");

            var relatedScreeningIds = _context.Screening.Where(s => s.movie_id == id).Select(s => s.screening_id).ToList();

            foreach (int i in relatedScreeningIds)
            {
                var relatedTickets = _context.Ticket.Where(t => t.screening_id == i);
                _context.Ticket.RemoveRange(relatedTickets);
                _context.SaveChanges();

                var screeningInQuestion = _context.Screening.Where(sc => sc.screening_id == i).ToList();
                _context.Screening.RemoveRange(screeningInQuestion); //there's just 1 but otherwise it's an IQueryable and it can't convert it
                _context.SaveChanges();
            }

            var imageToDelete = "img" + movieEntityEdit.poster_id + ".jpg";
            var pathToDelete = Path.Combine(www.WebRootPath, "Content", imageToDelete);
            if (System.IO.File.Exists(pathToDelete)) { System.IO.File.Delete(pathToDelete); }

            _context.Movie.Remove(movieEntityEdit);

            _context.SaveChanges();
            return RedirectToAction("RepertuarPanel");
        }






        //zgloszenie section

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddArticleDB(ArticleDTO articleDTO)
        {
            if (articleDTO == null) return RedirectToAction("ZgloszeniePanel");
            if (articleDTO.image.Length > 10485760) return RedirectToAction("ZgloszeniePanel");
            //                           ^10MB

            ArticleEntity newArticleEntity = new ArticleEntity();

            newArticleEntity.title = articleDTO.title;
            newArticleEntity.content = articleDTO.content;
            newArticleEntity.post_date = articleDTO.post_date;

            Random rngesus = new Random();
            var newImageIndex = rngesus.Next(0, 99999999);
            var newImageName = "img" + newImageIndex + ".jpg";
            var newImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Content", newImageName);

            using (var iostream = new FileStream(newImagePath, FileMode.Create)) articleDTO.image.CopyTo(iostream);

            newArticleEntity.image_id = newImageIndex;

            _context.Article.Add(newArticleEntity);
            _context.SaveChanges();

            return RedirectToAction("ZgloszeniePanel");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditArticleDB(ArticleDTO articleDTO)
        {
            var articleEntityEdit = _context.Article.FirstOrDefault(a => a.article_id == articleDTO.article_id);

            if (articleEntityEdit == null) return RedirectToAction("ZgloszeniePanel");

            if (!articleDTO.title.IsNullOrEmpty()) articleEntityEdit.title = articleDTO.title;
            if (!articleDTO.content.IsNullOrEmpty()) articleEntityEdit.content = articleDTO.content;
            articleEntityEdit.post_date = articleDTO.post_date;

            _context.SaveChanges();
            return RedirectToAction("ZgloszeniePanel");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteArticle(int id, [FromServices] IWebHostEnvironment www)
        {
            var articleEntityEdit = _context.Article.FirstOrDefault(a => a.article_id == id);
            
            if (articleEntityEdit == null) return RedirectToAction("ZgloszeniePanel");

            var imageToDelete = "img" + articleEntityEdit.image_id + ".jpg";
            var pathToDelete = Path.Combine(www.WebRootPath, "Content", imageToDelete);
            if (System.IO.File.Exists(pathToDelete)) { System.IO.File.Delete(pathToDelete); }

            _context.Article.Remove(articleEntityEdit);

            _context.SaveChanges();
            return RedirectToAction("ZgloszeniePanel");
        }
    }
}
