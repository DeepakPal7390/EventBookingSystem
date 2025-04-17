using System.ComponentModel.DataAnnotations;

namespace EventBookingSystem.Models.Domain
{
    public class User
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public ICollection<Booking>? Bookings { get; set; }
    }
}
