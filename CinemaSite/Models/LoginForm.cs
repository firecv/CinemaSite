using System.ComponentModel.DataAnnotations;

namespace CinemaSite.Models
{
    public class LoginForm
    {
        [Required, EmailAddress]
        public string email { get; set; }

        [Required]
        public string password_unhashed { get; set; }
    }
}
