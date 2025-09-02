using System.ComponentModel.DataAnnotations;

namespace CinemaSite.Models
{
    public class HallEntity
    {
        [Key]
        public int hall_id { get; set; }

        public bool imax { get; set; }

        public bool available { get; set; }

        public int rowsize { get; set; }
    }
}
