using CinemaSite.Data;
using CinemaSite.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            var viewModel = new DatabaseViewModel
            {
                Articles = _context.Article.OrderBy(a => a.article_id).ToList(),
                Movies = _context.Movie.OrderBy(m => m.movie_id).Take(10).ToList()
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
    }
}
