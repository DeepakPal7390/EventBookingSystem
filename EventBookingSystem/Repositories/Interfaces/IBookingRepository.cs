using EventBookingSystem.Models.Domain;

namespace EventBookingSystem.Repositories.Interfaces
{
    public interface IBookingRepository
    {
        Task<Booking> AddAsync(Booking booking);
        Task<bool> DeleteAsync(Guid bookingId);
    }
}
