using System.ComponentModel.DataAnnotations.Schema;

namespace EventBookingSystem.Models.Domain
{
    public class Booking
    {
        public Guid Id { get; set; }

        public string UserId { get; set; }

        public Guid EventId { get; set; }

        public long BookingTime { get; set; }

        [ForeignKey("EventId")]
        public Event Event { get; set; }
    }
}
