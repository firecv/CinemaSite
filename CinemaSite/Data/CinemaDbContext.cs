using Microsoft.EntityFrameworkCore;
using CinemaSite.Models;

namespace CinemaSite.Data
{
    public class CinemaDbContext:DbContext
    {
        public CinemaDbContext(DbContextOptions<CinemaDbContext> options) : base(options) { }

        public DbSet<TicketTypeEntity> TicketType { get; set; }
    }
}
