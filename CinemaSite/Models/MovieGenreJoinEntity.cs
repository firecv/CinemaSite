using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaSite.Models
{
    public class MovieGenreJoinEntity
    {
        [ForeignKey("MovieEntity")]
        public int movie_id { get; set; }

        [ForeignKey("GenreEntity")]
        public int genre_id { get; set; }
    }
}
