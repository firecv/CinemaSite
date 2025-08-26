using CinemaSite.Data;
using CinemaSite.Models;
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

        Cart cart = new Cart(); //TODO: change this to store it in cookies, then retrieve it from there as well

        public PurchaseController(CinemaDbContext context) => _context = context;

        [HttpPost]
        public async Task<IActionResult> CheckoutAsync([FromBody] Cart? cart = null)
        {
            try {
                var ticketTypePrices = await _context.TicketType.ToDictionaryAsync(tt => tt.ticket_type_id, tt => tt.price);
                var cartTotal = cart.tickets.Sum(t => ticketTypePrices[t.ticket_type_id]);

                var testTicket = await _context.Ticket.FirstOrDefaultAsync(t => t.ticket_id == 1);
                cart.tickets.Add(testTicket);

                var origin = $"{Request.Scheme}://{Request.Host}";


                var stripeSessionService = new SessionService();

                var checkoutSession = await stripeSessionService.CreateAsync(new SessionCreateOptions
                {
                    Mode = "payment",
                    ClientReferenceId = Guid.NewGuid().ToString(),
                    SuccessUrl = $"{origin}/Home/Index",
                    CancelUrl = $"{origin}/Home/Repertuar",
                    CustomerEmail = HttpContext.Session.GetString("ActiveUserEmail"),
                    LineItems = new()
                    {
                        new ()
                        {
                            PriceData = new()
                            {
                                Currency = "pln",
                                ProductData = new()
                                {
                                    Name = "Bilety"
                                },
                                UnitAmountDecimal = cartTotal
                            },
                            Quantity = 1
                        }
                    }
                });

                return Ok (new {redirectUrl = checkoutSession.Url});
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
