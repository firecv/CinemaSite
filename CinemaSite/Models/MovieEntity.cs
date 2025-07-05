using System.ComponentModel.DataAnnotations;

namespace CinemaSite.Models
{
    public class MovieEntity
    {
        [Key]
        public int movie_id { get; set; }

        public string title { get; set; }
        public string poster_id { get; set; }
        public bool for_kids { get; set; }
    }
}
