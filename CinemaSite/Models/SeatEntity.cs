using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaSite.Models
{
    public class SeatEntity
    {
        [Key]
        public int seat_id { get; set; }

        [ForeignKey("HallEntity")]
        public int hall_id { get; set; }

        [ForeignKey("SeatTypeEntity")]
        public int seat_type_id { get; set; }

        [Required]
        public int rowNum { get; set; }

        [Required]
        public int columnNum { get; set; }
    }
}
