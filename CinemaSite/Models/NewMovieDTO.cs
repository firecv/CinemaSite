using System.ComponentModel.DataAnnotations;

namespace CinemaSite.Models
{
    public class NewMovieDTO
    {
        public int movie_id { get; set; }

        public string? title { get; set; }
        public IFormFile? poster { get; set; }
        public bool for_kids { get; set; }
        public string? summary { get; set; }
        public string? trailer_link { get; set; }
        public List<int> genre_ids { get; set; }
    }
}
