using System.ComponentModel.DataAnnotations;

namespace CinemaSite.Models
{
    public class MovieDTO
    {
        public int movie_id { get; set; }

        public string? title { get; set; }
        //public int? poster_id { get; set; }
        public bool for_kids { get; set; }
        public string? summary { get; set; }
        public string? trailer_link { get; set; }
        public List<int> genre_ids { get; set; }
    }
}
