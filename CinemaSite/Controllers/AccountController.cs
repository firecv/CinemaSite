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

        public IActionResult NowyUzytkownik(NewUserAccount nowyUzytkownik)
        {
            if (ModelState.IsValid)
            {
                UserAccountEntity userAccount = new UserAccountEntity();

                userAccount.username = nowyUzytkownik.username;
                userAccount.email = nowyUzytkownik.email;
                userAccount.phone = nowyUzytkownik.phone;
                userAccount.password_hash = nowyUzytkownik.password_unhashed;
                // ^ this must be changed
                // HASH PASSWORDS HERE!!! DON'T FUCKING FORGET smth salt

                _context.UserAccount.Add(userAccount);
                _context.SaveChanges();
            }

            return RedirectToAction("Rejestracja");
        }
    }
}
