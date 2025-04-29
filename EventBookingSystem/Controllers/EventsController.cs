using EventBookingSystem.Constants;
using EventBookingSystem.Models.Domain;
using EventBookingSystem.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventBookingSystem.Controllers
{
    [ApiController]
    [Route(ApiRoutes.Events.Root)]
    public class EventsController : ControllerBase
    {
        private readonly IEventService _eventService;

        public EventsController(IEventService eventService)
        {
            _eventService = eventService;
        }

        [Authorize(Roles = "admin,User")]
        [HttpGet(ApiRoutes.Events.GetAll)]
        public async Task<IActionResult> GetAll()
        {
            var events = await _eventService.GetAllAsync();
            return Ok(events);
        }

        [Authorize(Roles = "admin")]
        [HttpGet(ApiRoutes.Events.GetById)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var ev = await _eventService.GetByIdAsync(id);
            if (ev == null) return NotFound();
            return Ok(ev);
        }

        [Authorize(Roles = "admin")]
        [HttpPost(ApiRoutes.Events.Add)]
        public async Task<IActionResult> Add([FromBody] Event ev)
        {
            var created = await _eventService.AddAsync(ev);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [Authorize(Roles = "admin")]
        [HttpPut(ApiRoutes.Events.Update)]
        public async Task<IActionResult> Update(Guid id, [FromBody] Event ev)
        {
            var updated = await _eventService.UpdateAsync(id, ev);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        [Authorize(Roles = "admin")]
        [HttpDelete(ApiRoutes.Events.Delete)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _eventService.DeleteAsync(id);
            if (deleted == null) return NotFound();
            return Ok(deleted);
        }
    }
}
