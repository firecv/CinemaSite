using CinemaSite.Data;
using CinemaSite.Models;
using CinemaSite.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe;
using Stripe.Checkout;

// https://www.youtube.com/watch?v=E9X2EAcvPHw&t=164s

namespace CinemaSite.Controllers
{
    [Route("checkout")]
    [ApiController]
    public class PurchaseController : Controller
    {
        private readonly CinemaDbContext _context;

        public PurchaseController(CinemaDbContext context) => _context = context;

        [HttpGet("stripe-checkout")]
        public IActionResult Checkout(int sumTotalCost, int screeningId, string ticketIdString)
        {
            var ticketIds = ticketIdString.Split(',').Select(int.Parse).ToList();

            var ticketsToConfirm = _context.Ticket.Where(t => ticketIds.Contains(t.ticket_id)).ToList();

            foreach (var ticket in ticketsToConfirm)
            {
                if (ticket.ticket_status == 0) ticket.ticket_status = 1;
            }

            _context.SaveChangesAsync();
            
            
            try {
                var stripeOptions = new Stripe.Checkout.SessionCreateOptions
                {
                    Mode = "payment",
                    ClientReferenceId = Guid.NewGuid().ToString(),
                    
                    //CustomerEmail = HttpContext.Session.GetString("ActiveUserEmail"),

                    SuccessUrl = "https://localhost:7273/Home/Konto",
                    CancelUrl = "https://localhost:7273/Home/Repertuar",
                    
                    LineItems = new()
                    {
                        new ()
                        {
                            PriceData = new SessionLineItemPriceDataOptions
                            {
                                Currency = "pln",
                                ProductData = new SessionLineItemPriceDataProductDataOptions
                                {
                                    Name = "Bilety"
                                },
                                UnitAmountDecimal = sumTotalCost
                            },
                            Quantity = 1
                        }
                    },

                    Metadata = new Dictionary<string, string>
                    {
                        { "ticket_ids", ticketIdString }
                    }
                };

                var stripeSessionService = new SessionService();
                Session session = stripeSessionService.Create(stripeOptions);

                Response.Headers.Add("Location", session.Url);
                return new StatusCodeResult(303); //immediately redirect to above url
            }
            catch (StripeException ex)
            {
                return StatusCode(502, $"Stripe error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server error: {ex.Message}");
            }
        }
    }
}
