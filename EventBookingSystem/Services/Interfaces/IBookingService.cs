using EventBookingSystem.Models.Domain;

namespace EventBookingSystem.Services.Interfaces
{
    public interface IBookingService
    {
        Task<Booking> AddBookingAsync(Booking booking);
        Task<bool> DeleteBookingAsync(Guid bookingId);
    }
}
