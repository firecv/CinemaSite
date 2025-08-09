using Microsoft.AspNetCore.Mvc;

namespace CinemaSite.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult NowyUzytkownik()
        {
            if (ModelState.IsValid)
            {

            }

            return RedirectToAction("Index");
        }
    }
}
