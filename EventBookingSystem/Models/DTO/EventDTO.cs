namespace EventBookingSystem.Models.DTO
{
    public class EventDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public string Location { get; set; }

        public long StartTime { get; set; }

        public long EndTime { get; set; }

        public int TotalSeats { get; set; }

        public int BookedSeats { get; set; }

    }
}
