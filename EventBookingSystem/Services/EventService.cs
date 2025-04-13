using EventBookingSystem.Models.Domain;
using EventBookingSystem.Repositories;

namespace EventBookingSystem.Services
{
    public class EventService : IEventService
    {    private readonly IEventRepositories _eventRepositories;

        public EventService(IEventRepositories eventRepositories)
        {
            _eventRepositories= eventRepositories;
        }
        public async Task<Event> AddAsync(Event ev)
        {   return await _eventRepositories.AddAsync(ev);
            //throw new NotImplementedException();
        }

        public async Task<Event?> DeleteAsync(Guid id)
        {
            return await _eventRepositories.DeleteAsync(id);
            //throw new NotImplementedException();
        }

        public async Task<IEnumerable<Event>> GetAllAsync()
        {
            return await _eventRepositories.GetAllAsync();
            throw new NotImplementedException();
        }

        public async Task<Event?> GetByIdAsync(Guid id)
        {    
            return await _eventRepositories.GetByIdAsync(id);
            //throw new NotImplementedException();
        }

        public async Task<Event?> UpdateAsync(Guid id, Event ev)
        {   return await _eventRepositories.UpdateAsync(id, ev);
            //throw new NotImplementedException();
        }
    }
}
