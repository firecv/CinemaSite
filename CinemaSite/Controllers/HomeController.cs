using System.Diagnostics;
using CinemaSite.Data;
using CinemaSite.Models;
using CinemaSite.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CinemaSite.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CinemaDbContext _context;
        public List<TicketTypeEntity> tickettypes = new List<TicketTypeEntity>();

        public HomeController(ILogger<HomeController> logger, CinemaDbContext context)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            var viewModel = new DatabaseViewModel
            {
                Articles = _context.Article.OrderBy(a => a.article_id).ToList(),
                Movies = _context.Movie.OrderBy(m => m.movie_id).Take(10).ToList()
            };

            return View(viewModel);
        }

        public IActionResult Repertuar(bool forKids = false)
        {
            if (forKids) {
                var viewModel = new DatabaseViewModel
                {
                    Movies = _context.Movie.AsQueryable().Where(m => m.for_kids).OrderBy(m => m.movie_id).ToList()
                };
                return View(viewModel);
            } else
            {
                var viewModel = new DatabaseViewModel
                {
                    Movies = _context.Movie.AsQueryable().OrderBy(m => m.movie_id).ToList()
                };
                return View(viewModel);
            }
        }

        public IActionResult Rejestracja(UserAccountEntity newUser) {
            if (ModelState.IsValid)
            {
                _context.UserAccount.Add(newUser);
                _context.SaveChanges();
                return RedirectToAction("Index"); // change this to user account page later, or add email verification
            } else
            {
                return View(); // alert user or refresh page maybe?
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
