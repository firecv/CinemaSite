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
        public IActionResult Checkout(int sumTotalCost, int screeningId)
        {
            try {
                var stripeOptions = new Stripe.Checkout.SessionCreateOptions
                {
                    Mode = "payment",
                    ClientReferenceId = Guid.NewGuid().ToString(),
                    
                    //CustomerEmail = HttpContext.Session.GetString("ActiveUserEmail"),

                    SuccessUrl = "https://docs.stripe.com/api/checkout/sessions/object?lang=dotnet", //placeholder links, will do it later
                    CancelUrl = "https://www.youtube.com/watch?v=MYb5TrjMCNU",
                    
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
                    }
                };

                var stripeSessionService = new SessionService();
                Session session = stripeSessionService.Create(stripeOptions);

                Response.Headers.Add("Location", session.Url);
                return new StatusCodeResult(303); //redirect to above url
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
