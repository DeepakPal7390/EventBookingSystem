using System;
using System.Threading.Tasks;

namespace EventBookingSystem.Services.Interfaces
{
    public interface IBookingService
    {
        Task<Guid> BookTicketAsync(Guid eventId, string userId);
        Task CancelBookingAsync(string userId, Guid eventId);

    }
}


