using EventBookingSystem.Models.Domain;
using EventBookingSystem.Repositories.Interfaces;
using EventBookingSystem.Services.Interfaces;

namespace EventBookingSystem.Services.Implementation
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;

        public BookingService(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }

        public async Task<Booking> AddBookingAsync(Booking booking)
        {
            return await _bookingRepository.AddAsync(booking);
        }

        public async Task<bool> DeleteBookingAsync(Guid bookingId)
        {
            return await _bookingRepository.DeleteAsync(bookingId);
        }
    }
}
