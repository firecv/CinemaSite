using System.ComponentModel.DataAnnotations;

namespace CinemaSite.Models
{
    public class NewUserAccount
    {
        [Key]
        public int account_id { get; set; }

        [Required]
        public string username { get; set; }

        [Required, EmailAddress]
        public string email { get; set; }

        [Required, Phone]
        public string phone { get; set; }

        [Required, MinLength(8), MaxLength(256)]
        public string password_unhashed { get; set; }

        [Required]
        [Compare("password_unhashed", ErrorMessage = "Hasła się nie zgadzają.")]
        public string repeat_password_unhashed { get; set; }
    }
}
