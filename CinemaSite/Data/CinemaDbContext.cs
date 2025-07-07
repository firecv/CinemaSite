using Microsoft.EntityFrameworkCore;
using CinemaSite.Models;

namespace CinemaSite.Data
{
    public class CinemaDbContext:DbContext
    {
        public CinemaDbContext(DbContextOptions<CinemaDbContext> options) : base(options) { }

        public DbSet<TicketTypeEntity> TicketType { get; set; }
        public DbSet<SeatTypeEntity> SeatType { get; set; }
        public DbSet<TicketEntity> Ticket { get; set; }
        public DbSet<SeatEntity> Seat { get; set; }
        public DbSet<UserAccountEntity> UserAccount { get; set; }
        public DbSet<ScreeningEntity> Screening { get; set; }
        public DbSet<NotificationEntity> Notification { get; set; }
        public DbSet<HallEntity> Hall { get; set; }
        public DbSet<MovieEntity> Movie { get; set; }
        public DbSet<ArticleEntity> Article { get; set; }
    }
}
