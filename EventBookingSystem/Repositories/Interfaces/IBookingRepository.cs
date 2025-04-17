using System.Threading.Tasks;
using EventBookingSystem.Models;
using EventBookingSystem.Models.Domain;

namespace EventBookingSystem.Repositories.Interfaces
{
    public interface IBookingRepository
    {
        Task AddAsync(Booking booking);
        
        Task<Booking?> GetByUserAndEventAsync(string userId, Guid eventId);
        Task DeleteAsync(Guid bookingId);

    }
}


