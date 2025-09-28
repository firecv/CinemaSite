using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.IO;
using Stripe;
using CinemaSite.Data;

namespace CinemaSite.Controllers
{

    [ApiController]
    [Route("api/stripe")]
    public class WebhookController : Controller
    {
        private string apiKey;
        private CinemaDbContext _context;
        private string webhookSecret;

        public WebhookController(IConfiguration config, CinemaDbContext context) {
            apiKey = config["Stripe:Secretkey"];
            _context = context;
            webhookSecret = config["Stripe:WebhookSecret"];
        }

        [HttpPost]
        public async Task<IActionResult> AlterTickets()
        {
            var stripeJsonResponse = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            try
            {
                Event stripeEvent;

                try
                {
                    var signature = Request.Headers["Stripe-Signature"];
                    stripeEvent = EventUtility.ConstructEvent(stripeJsonResponse, signature, webhookSecret);
                } catch (StripeException se)
                {
                    return BadRequest(se.Message);
                }


                if (stripeEvent.Type == "checkout.session.completed")
                {
                    var stripeSession = stripeEvent.Data.Object as Stripe.Checkout.Session;

                    if (stripeSession != null && stripeSession.Metadata.ContainsKey("ticket_ids"))
                    {
                        string ticketIdString = stripeSession.Metadata["ticket_ids"];
                        var ticketIds = ticketIdString.Split(',').Select(int.Parse).ToList();

                        var ticketsToConfirm = _context.Ticket.Where(t => ticketIds.Contains(t.ticket_id)).ToList();

                        foreach (var ticket in ticketsToConfirm)
                        {
                            if (ticket.ticket_status == 1) ticket.ticket_status = 2;
                        }

                        await _context.SaveChangesAsync();
                    }
                }
            
                return Ok();
            }
            catch (StripeException se)
            {
                return BadRequest(se.Message);
            }
        }
    }
}
