using System.ComponentModel.DataAnnotations;

namespace CinemaSite.Models
{
    public class ArticleDTO
    {
        [Key]
        public int article_id { get; set; }

        public IFormFile? image { get; set; }

        [Required]
        public string content { get; set; }

        public DateTime post_date { get; set; }

        public string title { get; set; }
    }
}
