using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaSite.Models
{
    public class ScreeningEntity
    {
        [Key]
        public int screening_id { get; set; }

        [ForeignKey("MovieEntity")]
        public int movie_id { get; set; }

        [ForeignKey("HallEntity")]
        public int hall_id { get; set; }

        [Required]
        public DateTime screening_time { get; set; }
    }
}
