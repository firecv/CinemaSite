using CinemaSite.Models;
using Microsoft.AspNetCore.Mvc;

namespace CinemaSite.ViewModels
{
    public class AccountViewModel
    {
        public List<UserAccountEntity> UserAccounts { get; set; }

        public string username { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string password_unhashed { get; set; }
        public string repeat_password_unhashed { get; set; }

    }
}
