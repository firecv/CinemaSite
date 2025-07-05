using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaSite.Models
{
    public class NotificationEntity
    {
        [Key]
        public int notification_id { get; set; }

        [ForeignKey("UserAccountEntity")]
        public int account_id { get; set; }

        [Required]
        public string content { get; set; }

        [Required]
        public bool is_read { get; set; }
    }
}
