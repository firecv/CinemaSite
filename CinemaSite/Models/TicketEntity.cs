using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaSite.Models
{
    public class TicketEntity
    {
        [Key]
        public int ticket_id { get; set; }

        [ForeignKey("ScreeningEntity")]
        public int screening_id { get; set; }

        [ForeignKey("UserAccountEntity")]
        public int account_id { get; set; }

        [ForeignKey("SeatEntity")]
        public int seat_id { get; set; }

        [ForeignKey("TicketTypeEntity")]
        public int ticket_type_id { get; set; }
        
        public int ticket_status { get; set; }
        // 0 = pending (in cart) - should be deleted after like 15-30 minutes
        // 1 = in process of purchasing
        // 2 = purchased, valid
    }
}
