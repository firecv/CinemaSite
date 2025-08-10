using System.ComponentModel.DataAnnotations;

namespace CinemaSite.Models
{
    public class UserAccountEntity
    {
        [Key]
        public int account_id { get; set; }

        [Required]
        public string username { get; set; }

        [Required, EmailAddress] 
        public string email { get; set; }

        [Required, Phone] 
        public string phone { get; set; }

        [Required]
        public string password_hash { get; set; }
    }
}
