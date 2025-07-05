using System.Diagnostics;
using CinemaSite.Data;
using CinemaSite.Models;
using Microsoft.AspNetCore.Mvc;

namespace CinemaSite.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CinemaDbContext _context;

        public HomeController(ILogger<HomeController> logger, CinemaDbContext context)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            List<TicketTypeEntity> tickettypes = new List<TicketTypeEntity>();
            tickettypes = _context.TicketType.ToList();
            return View();
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
