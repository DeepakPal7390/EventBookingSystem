using System.Threading.Tasks;
using EventBookingSystem.Data;
using EventBookingSystem.Models;
using EventBookingSystem.Models.Domain;
using EventBookingSystem.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EventBookingSystem.Repositories.Implementation
{
    public class BookingRepository : IBookingRepository
    {
        private readonly EventDbContext _context;

        public BookingRepository(EventDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Booking booking)
        {
            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();
        }

        public async Task<Booking?> GetByUserAndEventAsync(string userId, Guid eventId)
        {
            return await _context.Bookings
                                 .FirstOrDefaultAsync(b => b.UserId == userId && b.EventId == eventId);
        }

        public async Task DeleteAsync(Guid bookingId)
        {
            var booking = await _context.Bookings.FindAsync(bookingId);
            if (booking != null)
            {
                _context.Bookings.Remove(booking);
                await _context.SaveChangesAsync();
            }
        }

    }
}


