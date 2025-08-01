using System.ComponentModel.DataAnnotations;

namespace CinemaSite.Models
{
    public class ArticleEntity
    {
        [Key]
        public int article_id { get; set; }

        public int image_id { get; set; }

        [Required]
        public string content { get; set; }

        public DateTime post_date { get; set; }

        public string title { get; set; }
    }
}
