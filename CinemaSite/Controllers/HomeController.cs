using System.Diagnostics;
using CinemaSite.Data;
using CinemaSite.Models;
using CinemaSite.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

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
                Articles = _context.Article.Where(a => a.post_date <= DateTime.Now)
                .OrderByDescending(a => a.post_date).Take(5).ToList(),
                Movies = _context.Movie.OrderByDescending(m => m.movie_id).Take(10).ToList()
            };

            return View(viewModel);
        }

        public IActionResult Cennik()
        {
            var viewModel = new DatabaseViewModel
            {
                TicketTypes = _context.TicketType.OrderByDescending(tt => tt.price).ToList()
            };

            return View(viewModel);
        }

        public IActionResult Zgloszenia()
        {
            var viewModel = new DatabaseViewModel
            {
                Articles = _context.Article.Where(a => a.post_date <= DateTime.Now)
                .OrderByDescending(a => a.post_date).ToList()
            };

            return View(viewModel);
        }


        public IActionResult Repertuar()
        {
            var upcomingScreenings = _context.Screening
                .Where(s => s.screening_time >= DateTime.Now && s.screening_time <= DateTime.Now.AddDays(14))
                .OrderBy(s => s.screening_time)
                .AsNoTracking().ToList();

            var upcomingMovieIds = upcomingScreenings
                .Select(s => s.movie_id)
                .Distinct()
                .ToList();

            var upcomingMovies = _context.Movie
                .Where(m => upcomingMovieIds.Contains(m.movie_id))
                .OrderBy(m => m.movie_id)
                .AsNoTracking().ToList();

            var neededMovieGenreJoins = _context.MovieGenre
                .Where(mg => upcomingMovieIds.Contains(mg.movie_id))
                .ToList();

            var viewModel = new DatabaseViewModel
            {
                Screenings = upcomingScreenings,
                Movies = upcomingMovies,
                Genres = _context.Genre.ToList(),
                MovieGenreJoins = neededMovieGenreJoins
            };
            return View(viewModel);
        }




        public IActionResult Rezerwacja(int movieId, int screeningId)
        {
            var movie = _context.Movie.FirstOrDefault(m => m.movie_id == movieId);
            var screening = _context.Screening.FirstOrDefault(s => s.screening_id == screeningId);
            var hall = _context.Hall.FirstOrDefault(h => h.hall_id == screening.hall_id);

            if (movie == null || screening == null || hall == null) { return Repertuar(); }

            var seatsInHall = _context.Seat
                .Where(s => s.hall_id == screening.hall_id)
                .ToList();

            var ticketsRelated = _context.Ticket
                .Where(t => t.screening_id == screening.screening_id
                && (t.ticket_status == 2 || t.hold_until > DateTime.UtcNow))
                .ToList();

            var viewModel = new RezerwacjaViewModel
            {
                Movie = movie,
                Screening = screening,
                Hall = hall,
                Seats = seatsInHall,
                SeatTypes = _context.SeatType.ToList(),
                Tickets = ticketsRelated,
                TicketTypes = _context.TicketType.ToList()
            };

            return View(viewModel);
        }




        [HttpPost]
        public IActionResult RedirectCheckout(int screeningIdPost, int movieIdPost, List<int> ticketTypesPost, List<int> seatIdsPost)
        {
            if (HttpContext.Session.GetInt32("ActiveUserID") == null || screeningIdPost == 0)
            {
                return RedirectToAction("Logowanie", "Account",
                    new { redirectMovieId = movieIdPost, redirectScreeningId = screeningIdPost });
            }

            Console.WriteLine(screeningIdPost);

            var ticketTypesDict = _context.TicketType.ToDictionary(tt => tt.ticket_type_id, tt => tt.price);
            var sumTotalCost = 0;
            var currentUser = (int)HttpContext.Session.GetInt32("ActiveUserID");

            for (int i = 0; i < ticketTypesPost.Count(); i++)
            {
                TicketEntity newCartTicket = new TicketEntity();

                newCartTicket.screening_id = screeningIdPost;
                newCartTicket.account_id = currentUser;
                newCartTicket.seat_id = seatIdsPost[i];
                newCartTicket.ticket_type_id = ticketTypesPost[i];
                newCartTicket.ticket_status = 0;
                newCartTicket.hold_until = DateTime.UtcNow.AddMinutes(15);

                _context.Ticket.Add(newCartTicket);

                if (ticketTypesDict.TryGetValue(ticketTypesPost[i], out var tp))
                {
                    sumTotalCost += tp;
                } else
                {
                    //TODO: add some better error handling later
                    return View();
                }
            }

            _context.SaveChanges();

            var ticketsForPurchase = _context.Ticket
                .OrderByDescending(t => t.ticket_id)
                .Where(t => t.account_id == currentUser)
                .Take(ticketTypesPost.Count())
                .Select(t => t.ticket_id)
                .ToList();

            return RedirectToAction(
                controllerName: "Purchase",
                actionName: "Checkout",
                routeValues: new
                {
                    sumTotalCost = sumTotalCost,
                    screeningId = screeningIdPost,
                    ticketIdString = string.Join(",", ticketsForPurchase)
                }
            );
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
