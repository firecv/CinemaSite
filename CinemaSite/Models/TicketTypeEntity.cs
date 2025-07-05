using System.ComponentModel.DataAnnotations;

namespace CinemaSite.Models
{
    public class TicketTypeEntity
    {
        [Key]
        public int ticket_type_id { get; set; }

        [Required]
        public string ticket_type { get; set; }
    }
}
