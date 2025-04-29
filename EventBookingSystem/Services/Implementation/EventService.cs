using System.Text.Json;
using EventBookingSystem.Models.Domain;
using EventBookingSystem.Repositories.Interfaces;
using EventBookingSystem.Services.Interfaces;
using EventBookingSystem.Cache; 
using Microsoft.Extensions.Logging;

namespace EventBookingSystem.Services.Implementation
{
    public class EventService : IEventService
    {
        private const string EventsCacheKey = "all_events";
        private readonly IEventRepositories _eventRepositories;
        private readonly ILogger<EventService> _logger;
        private readonly ICacheService _cacheService; 

        public EventService(
            IEventRepositories eventRepositories,
            ILogger<EventService> logger,
            ICacheService cacheService) 
        {
            _eventRepositories = eventRepositories;
            _logger = logger;
            _cacheService = cacheService;
        }

        public async Task<Event> AddAsync(Event ev)
        {
            _logger.LogInformation("Attempting to add new event: {Title}", ev.Title);
            var result = await _eventRepositories.AddAsync(ev);
            _logger.LogInformation("Event added successfully. EventId: {EventId}", result.Id);

            await _cacheService.RemoveAsync(EventsCacheKey);
            return result;
        }

        public async Task<Event?> DeleteAsync(Guid id)
        {
            _logger.LogInformation("Attempting to delete event. EventId: {EventId}", id);
            var result = await _eventRepositories.DeleteAsync(id);

            if (result != null)
            {
                _logger.LogInformation("Event deleted successfully. EventId: {EventId}", id);
                await _cacheService.RemoveAsync(EventsCacheKey);
            }
            else
            {
                _logger.LogWarning("Event not found for deletion. EventId: {EventId}", id);
            }

            return result;
        }

        public async Task<IEnumerable<Event>> GetAllAsync()
        {
            _logger.LogInformation("Fetching all events");

            var cachedEvents = await _cacheService.GetAsync<IEnumerable<Event>>(EventsCacheKey);
            if (cachedEvents != null)
            {
                _logger.LogInformation("Returned events from cache.");
                return cachedEvents;
            }

            var events = await _eventRepositories.GetAllAsync();
            await _cacheService.SetAsync(EventsCacheKey, events, TimeSpan.FromMinutes(5));

            _logger.LogInformation("Events fetched from DB and cached. Count: {Count}", events.Count());
            return events;
        }

        public async Task<Event?> GetByIdAsync(Guid id)
        {
            _logger.LogInformation("Fetching event by ID: {EventId}", id);
            var ev = await _eventRepositories.GetByIdAsync(id);
            if (ev != null)
            {
                _logger.LogInformation("Event found: {Title}", ev.Title);
            }
            else
            {
                _logger.LogWarning("Event not found. EventId: {EventId}", id);
            }

            return ev;
        }

        public async Task<Event?> UpdateAsync(Guid id, Event ev)
        {
            _logger.LogInformation("Attempting to update event. EventId: {EventId}", id);
            var result = await _eventRepositories.UpdateAsync(id, ev);

            if (result != null)
            {
                _logger.LogInformation("Event updated successfully. EventId: {EventId}", id);
                await _cacheService.RemoveAsync(EventsCacheKey);
            }
            else
            {
                _logger.LogWarning("Event not found for update. EventId: {EventId}", id);
            }

            return result;
        }
    }
}

