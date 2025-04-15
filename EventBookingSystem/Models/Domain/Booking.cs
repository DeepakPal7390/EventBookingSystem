using System.ComponentModel.DataAnnotations.Schema;

namespace EventBookingSystem.Models.Domain
{
    public class Booking
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public Guid EventId { get; set; }

        public long BookingTime { get; set; }

        //[ForeignKey("UserId")]
        //public User User { get; set; }

        //[ForeignKey("EventId")]
        //public Event Event { get; set; }
    }
}
