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
                    Movies = _context.Movie.AsQueryable().Where(m => m.for_kids).OrderBy(m => m.movie_id).ToList(),
                    Screenings = _context.Screening.ToList()
                };
                return View(viewModel);
            } else
            {
                var viewModel = new DatabaseViewModel
                {
                    Movies = _context.Movie.AsQueryable().OrderBy(m => m.movie_id).ToList(),
                    Screenings = _context.Screening.ToList()
                };
                return View(viewModel);
            }
        }

        public IActionResult Konto()
        {
            var UserTickets = from t in _context.Ticket
                              join sc in _context.Screening on t.screening_id equals sc.screening_id
                              join m in _context.Movie on sc.movie_id equals m.movie_id
                              join h in _context.Hall on sc.hall_id equals h.hall_id
                              join tt in _context.TicketType on t.ticket_type_id equals tt.ticket_type_id
                              join s in _context.Seat on t.seat_id equals s.seat_id
                              orderby t.ticket_id descending
                              select new UserTicket {
                                  ticket_id = t.ticket_id,
                                  datetime = sc.screening_time,
                                  ticket_type = tt.ticket_type,
                                  title = m.title,
                                  hall_id = h.hall_id,
                                  rowNum = s.rowNum,
                                  columnNum = s.columnNum,
                              };

            return View(UserTickets.ToList());
        }

        public IActionResult Rejestracja()
        {
            var viewModel = new AccountViewModel
            {
                UserAccounts = _context.UserAccount.OrderBy(u => u.account_id).ToList()
            };

            return View(viewModel);
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
