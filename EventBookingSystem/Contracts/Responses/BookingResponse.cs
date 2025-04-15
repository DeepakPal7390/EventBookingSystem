namespace EventBookingSystem.Contracts.Responses
{
    public class BookingResponse
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }   
        public Guid EventId { get; set; }  
        public long BookingTime { get; set; }
    }
}
