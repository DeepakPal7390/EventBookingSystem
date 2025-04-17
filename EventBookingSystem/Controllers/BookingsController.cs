using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using EventBookingSystem.Services.Interfaces;

namespace EventBookingSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [Authorize]
        [HttpPost("{eventId}")]
        public async Task<IActionResult> BookTicket(Guid eventId)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized("Invalid token: userId not found.");

            var result = await _bookingService.BookTicketAsync(eventId, userId);
            return Ok(result);
        }

        [Authorize]
        [HttpDelete("cancel")]
        public async Task<IActionResult> CancelBooking([FromQuery] Guid eventId)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("Invalid token: userId not found.");

            var result = await _bookingService.CancelBookingAsync(userId, eventId);
            return Ok(result);
        }

    }
}


