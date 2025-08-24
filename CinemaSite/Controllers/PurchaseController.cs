using CinemaSite.Data;
using Microsoft.AspNetCore.Mvc;

namespace CinemaSite.Controllers
{
    [Route("checkout")]
    public class PurchaseController : Controller
    {
        private readonly CinemaDbContext _context;

        public PurchaseController(CinemaDbContext context) => _context = context;

        //TODO: Add checkout depending on Cart
    }
}
