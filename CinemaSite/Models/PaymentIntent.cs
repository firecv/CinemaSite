using System.ComponentModel.DataAnnotations;

namespace CinemaSite.Models
{
    public class PaymentIntent
    {
        [Required]
        public string stripe_session_id { get; set; }

        [Required]
        public string stripe_payment_id { get; set; }

        [Required]
        public string payment_status { get; set; }

        [Required]
        public DateTime payment_timestamp { get; set; }
    }
}
