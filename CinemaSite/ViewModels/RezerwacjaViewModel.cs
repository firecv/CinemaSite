using CinemaSite.Models;
using Microsoft.AspNetCore.Mvc;

namespace CinemaSite.ViewModels
{
    public class RezerwacjaViewModel
    {
        public MovieEntity Movie { get; set; }
        public ScreeningEntity Screening { get; set; }
        public List<SeatEntity> Seats { get; set; }
        public List<TicketEntity> Tickets { get; set; }
    }
}
