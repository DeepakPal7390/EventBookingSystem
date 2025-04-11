using EventBookingSystem.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace EventBookingSystem.Data
{
    public class EventDbContext : DbContext
    {
        public EventDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {
            
        }
        public DbSet<Event> Events { get; set; }
    }
}
