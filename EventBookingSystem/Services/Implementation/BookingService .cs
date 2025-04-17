using System;
using System.Threading.Tasks;
using EventBookingSystem.Repositories.Interfaces;
using EventBookingSystem.Services.Interfaces;
using EventBookingSystem.Models;
using EventBookingSystem.Models.Domain;

namespace EventBookingSystem.Services.Implementation
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IEventRepositories _eventRepository;

        public BookingService(IBookingRepository bookingRepository, IEventRepositories eventRepository)
        {
            _bookingRepository = bookingRepository;
            _eventRepository = eventRepository;
        }

        public async Task<string> BookTicketAsync(Guid eventId, string userId)
        {
            var ev = await _eventRepository.GetByIdAsync(eventId);
            if (ev == null) return "Event not found.";
            if (ev.BookedSeats >= ev.TotalSeats) return "Seats are not available.";

            ev.BookedSeats += 1;
            await _eventRepository.UpdateAsync(eventId, ev);

            var booking = new Booking
            {
                Id = Guid.NewGuid(),
                EventId = eventId,
                UserId = userId,
                BookingTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
            };

            await _bookingRepository.AddAsync(booking);
            return "Seat booked successfully.";
        }




       public async Task<string> CancelBookingAsync(string userId, Guid eventId)
        {
            var booking = await _bookingRepository.GetByUserAndEventAsync(userId, eventId);
            if (booking == null)
                return "Booking not found.";

            var ev = await _eventRepository.GetByIdAsync(eventId);
            if (ev == null)
                return "Event not found.";

            ev.BookedSeats -= 1;
            await _eventRepository.UpdateAsync(eventId, ev);

            await _bookingRepository.DeleteAsync(booking.Id);
            return "Booking cancelled successfully.";
        }

    }
}



