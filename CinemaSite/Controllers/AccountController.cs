using CinemaSite.Data;
using CinemaSite.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Threading.Tasks;

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

        public async Task<IActionResult> NowyUzytkownik(NewUserAccount nowyUzytkownik)
        {
            var _hasher = new PasswordHasher<UserAccountEntity>();

            bool accountExists = 
                await _context.UserAccount.AnyAsync(u => u.username == nowyUzytkownik.username || u.email == nowyUzytkownik.email);

            if (ModelState.IsValid && !accountExists)
            {
                UserAccountEntity userAccount = new UserAccountEntity();

                userAccount.username = nowyUzytkownik.username;
                userAccount.email = nowyUzytkownik.email;
                userAccount.phone = nowyUzytkownik.phone;

                userAccount.password_hash = _hasher.HashPassword(null, nowyUzytkownik.password_unhashed);

                // (Sprawdzenie czy poprawnie hashowane)
                if (userAccount.password_hash == null || userAccount.password_hash == nowyUzytkownik.password_unhashed) {
                    throw new Exception("Password was not hashed.");
                }

                _context.UserAccount.Add(userAccount);
                _context.SaveChanges();

                return RedirectToAction("Rejestracja");
            }

            return RedirectToAction("Rejestracja"); //add message to say that account already exists
        }
    }
}
