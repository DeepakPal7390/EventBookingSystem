using EventBookingSystem.Data;
using EventBookingSystem.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace EventBookingSystem.Repositories
{
    public class EventRepositories : IEventRepositories
    {

        private readonly EventDbContext dbContext;

        public EventRepositories(EventDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Event> AddAsync(Event ev)
        {
            await dbContext.Events.AddAsync(ev);
            await dbContext.SaveChangesAsync();
            return ev;
           
        }

        public async Task<Event?> DeleteAsync(Guid id)
        {
            var existing = await dbContext.Events.FindAsync(id);
            if (existing == null) return null;

            dbContext.Events.Remove(existing);
            await dbContext.SaveChangesAsync();
            return existing;
            //throw new NotImplementedException();
        }

        public async Task<IEnumerable<Event>> GetAllAsync()
        {
            return await dbContext.Events.ToListAsync();

            //throw new NotImplementedException();
        }

        public async Task<Event?> GetByIdAsync(Guid id)
        {
            return await dbContext.Events.FindAsync(id);
            //throw new NotImplementedException();
        }

        public  async Task<Event?> UpdateAsync(Guid id, Event ev)
        {
            var existing = await dbContext.Events.FindAsync(id);
            if (existing == null) return null;

            existing.Title = ev.Title;
            existing.Description = ev.Description;
            existing.Location = ev.Location;
            existing.StartTime = ev.StartTime;
            existing.EndTime = ev.EndTime;
            existing.TotalSeats = ev.TotalSeats;
            existing.BookedSeats = ev.BookedSeats;

            await dbContext.SaveChangesAsync();
            return existing;
            //throw new NotImplementedException();
        }
    }
}
