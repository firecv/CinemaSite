using System.ComponentModel.DataAnnotations;

namespace CinemaSite.Models
{
    public class GenreEntity
    {
        [Key]
        public int genre_id { get; set; }

        [Required]
        public string genre_name { get; set; }
    }
}
