using System.ComponentModel.DataAnnotations;

namespace CinemaSite.Models
{
    public class SeatTypeEntity
    {
        [Key]
        public int seat_type_id { get; set; }

        [Required]
        public string seat_type { get; set; }
    }
}
