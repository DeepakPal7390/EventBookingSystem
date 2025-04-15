using EventBookingSystem.Models.Domain;
using EventBookingSystem.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventBookingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingsController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        
        [HttpPost]
        public async Task<IActionResult> AddBooking([FromBody] Booking booking)
        {
            var createdBooking = await _bookingService.AddBookingAsync(booking);
            //return CreatedAtAction(nameof(GetById), new { id = createdBooking.Id }, createdBooking);
            return CreatedAtAction("", createdBooking);

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooking(Guid id)
        {
            var result = await _bookingService.DeleteBookingAsync(id);
            if (!result) return NotFound();

            return NoContent();
        }
    }
}
