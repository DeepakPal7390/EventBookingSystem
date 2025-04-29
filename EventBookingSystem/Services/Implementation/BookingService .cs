using System;
using System.Threading.Tasks;
using EventBookingSystem.Repositories.Interfaces;
using EventBookingSystem.Services.Interfaces;
using EventBookingSystem.Models.Domain;
using EventBookingSystem.Exceptions;
using Microsoft.Extensions.Logging;

namespace EventBookingSystem.Services.Implementation
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IEventRepositories _eventRepository;
        private readonly ILogger<BookingService> _logger;

        public BookingService(
            IBookingRepository bookingRepository,
            IEventRepositories eventRepository,
            ILogger<BookingService> logger)
        {
            _bookingRepository = bookingRepository;
            _eventRepository = eventRepository;
            _logger = logger;
        }

        public async Task<Guid> BookTicketAsync(Guid eventId, string userId)
        {
            _logger.LogInformation("Booking attempt started for EventId: {EventId}, UserId: {UserId}", eventId, userId);

            var ev = await _eventRepository.GetByIdAsync(eventId);
            if (ev == null)
            {
                _logger.LogWarning("Event not found. EventId: {EventId}", eventId);
                throw new NotFoundException("Event not found.");
            }

            if (ev.BookedSeats >= ev.TotalSeats)
            {
                _logger.LogWarning("Booking failed. No seats available. EventId: {EventId}", eventId);
                throw new BadRequestException("Seats are not available.");
            }

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
            _logger.LogInformation("Booking successful. BookingId: {BookingId}, UserId: {UserId}, EventId: {EventId}", booking.Id, userId, eventId);

            return booking.Id;
        }

        public async Task CancelBookingAsync(string userId, Guid eventId)
        {
            _logger.LogInformation("Cancel booking request received. EventId: {EventId}, UserId: {UserId}", eventId, userId);

            var booking = await _bookingRepository.GetByUserAndEventAsync(userId, eventId);
            if (booking == null)
            {
                _logger.LogWarning("Booking not found for cancellation. EventId: {EventId}, UserId: {UserId}", eventId, userId);
                throw new NotFoundException("Booking not found.");
            }

            var ev = await _eventRepository.GetByIdAsync(eventId);
            if (ev == null)
            {
                _logger.LogWarning("Event not found during cancellation. EventId: {EventId}", eventId);
                throw new NotFoundException("Event not found.");
            }

            ev.BookedSeats -= 1;
            await _eventRepository.UpdateAsync(eventId, ev);

            await _bookingRepository.DeleteAsync(booking.Id);

            _logger.LogInformation("Booking cancelled successfully. BookingId: {BookingId}, EventId: {EventId}, UserId: {UserId}", booking.Id, eventId, userId);
        }
    }
}



