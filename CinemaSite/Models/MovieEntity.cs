using System.ComponentModel.DataAnnotations;

namespace CinemaSite.Models
{
    public class MovieEntity
    {
        [Key]
        public int movie_id { get; set; }

        public string tmdb_id { get; set; }

        public bool for_kids { get; set; }
    }
}
