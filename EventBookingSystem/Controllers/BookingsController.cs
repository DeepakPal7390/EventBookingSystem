using EventBookingSystem.Constants;
using EventBookingSystem.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route(ApiRoutes.Bookings.Root)]
public class BookingController : ControllerBase
{
    private readonly IBookingService _bookingService;

    public BookingController(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    [Authorize(Roles = "User")]
    [HttpPost(ApiRoutes.Bookings.Book)]
    public async Task<IActionResult> BookTicket(Guid eventId)
    {
        var userId = HttpContext.Items["UserId"]?.ToString();
        if (string.IsNullOrEmpty(userId))
            return Unauthorized("User ID not found in token.");

     var bookingId=   await _bookingService.BookTicketAsync(eventId, userId); 
        return Ok(new
        {
            Message = "Seat booked successfully.",
            BookingId = bookingId
        });
    }

    [Authorize(Roles = "User")]
    [HttpDelete(ApiRoutes.Bookings.Cancel)]
    public async Task<IActionResult> CancelBooking([FromQuery] Guid eventId)
    {
        var userId = HttpContext.Items["UserId"]?.ToString();
        if (string.IsNullOrEmpty(userId))
            return Unauthorized("User ID not found in token.");

        await _bookingService.CancelBookingAsync(userId, eventId);
        return Ok("Booking cancelled successfully."); 
    }
}
