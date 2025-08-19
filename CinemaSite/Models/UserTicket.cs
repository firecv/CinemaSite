namespace CinemaSite.Models
{
    public class UserTicket
    {
        public int ticket_id { get; set; }
        public DateTime datetime { get; set; }
        public string ticket_type { get; set; }
        public string title { get; set; }
        public int hall_id { get; set; }
        public char rowNum { get; set; }
        public int columnNum { get; set; }
    }
}
