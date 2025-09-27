using CinemaSite.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace CinemaSite.ViewModels
{
    public class AdminViewModel
    {
        public List<TicketTypeEntity> TicketTypes { get; set; }
        public List<SeatTypeEntity> SeatTypes { get; set; }
        public List<TicketEntity> Tickets { get; set; }
        public List<SeatEntity> Seats { get; set; }
        public List<UserAccountEntity> UserAccounts { get; set; }
        public List<ScreeningEntity> Screenings { get; set; }
        public List<NotificationEntity> Notifications { get; set; }
        public List<HallEntity> Halls { get; set; }
        public List<MovieEntity> Movies { get; set; }
        public List<ArticleEntity> Articles { get; set; }
        public List<GenreEntity> Genres { get; set; }
        public List<MovieGenreJoinEntity> MovieGenreJoins { get; set; }
        public MovieDTO MovieEdit { get; set; }
        public NewMovieDTO NewMovie { get; set; }
        public ArticleDTO ArticleDTO { get; set; }
    }
}
