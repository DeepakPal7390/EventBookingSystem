using EventBookingSystem.Models.Domain;

namespace EventBookingSystem.Services.Interfaces
{
    public interface IEventService
    {
         Task<IEnumerable<Event>> GetAllAsync();
        Task<Event?> GetByIdAsync(Guid id);
        Task<Event> AddAsync(Event ev);
        Task<Event?> UpdateAsync(Guid id, Event ev);
        Task<Event?> DeleteAsync(Guid id);
    }
}
