using CinemaSite.Data;
using CinemaSite.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CinemaSite.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly CinemaDbContext _context;

        public AccountController(ILogger<AccountController> logger, CinemaDbContext context)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Rejestracja()
        {
            return View();
        }

        public IActionResult NowyUzytkownik(UserAccountEntity nowyUzytkownik)
        {
            if (ModelState.IsValid)
            {
                _context.UserAccount.Add()
            }

            return RedirectToAction("Rejestracja");
        }
    }
}
