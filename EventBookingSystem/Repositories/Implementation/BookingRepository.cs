using EventBookingSystem.Data;
using EventBookingSystem.Models.Domain;
using EventBookingSystem.Repositories.Interfaces;

namespace EventBookingSystem.Repositories.Implementation
{
    public class BookingRepository : IBookingRepository
    {
        private readonly EventDbContext dbContext;

        public BookingRepository(EventDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<Booking> AddAsync(Booking booking)
        {
            await dbContext.Bookings.AddAsync(booking);
            await dbContext.SaveChangesAsync();
            return booking;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var existing = await dbContext.Bookings.FindAsync(id);
            if (existing == null) return false;

            dbContext.Bookings.Remove(existing);

            await dbContext.SaveChangesAsync();
            return true;
        }
    }
}
