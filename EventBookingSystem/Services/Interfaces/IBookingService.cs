using System;
using System.Threading.Tasks;

namespace EventBookingSystem.Services.Interfaces
{
    public interface IBookingService
    {
        Task<string> BookTicketAsync(Guid eventId, string userId);
        //Task<string> CancelBookingAsync(Guid bookingId, string userId);
        Task<string> CancelBookingAsync(string userId, Guid eventId);

    }
}


