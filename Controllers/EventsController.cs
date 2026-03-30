using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EventManagerAPI.Data;
using EventManagerAPI.Models;
using EventManagerAPI.DTOs;

namespace EventManagerAPI.Controllers
{
    [ApiController]
    [Route("api/events")]
    public class EventsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EventsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetEvents()
        {
            var events = await _context.Events
                .Include(e => e.Attendees)
                .ToListAsync();

            var result = events.Select(e => new EventDto
            {
                Id = e.Id,
                Title = e.Title,
                Description = e.Description,
                Date = e.Date,
                Location = e.Location,
                Attendees = e.Attendees.Select(a => new AttendeeDto
                {
                    Id = a.Id,
                    Name = a.Name,
                    Email = a.Email
                }).ToList()
            });

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEvent(int id)
        {
            var ev = await _context.Events
                .Include(e => e.Attendees)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (ev == null) return NotFound();

            var result = new EventDto
            {
                Id = ev.Id,
                Title = ev.Title,
                Description = ev.Description,
                Date = ev.Date,
                Location = ev.Location,
                Attendees = ev.Attendees.Select(a => new AttendeeDto
                {
                    Id = a.Id,
                    Name = a.Name,
                    Email = a.Email
                }).ToList()
            };

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEvent(CreateEventDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var ev = new Event
            {
                Title = dto.Title,
                Description = dto.Description,
                Date = dto.Date,
                Location = dto.Location
            };

            _context.Events.Add(ev);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEvent), new { id = ev.Id }, new EventDto
            {
                Id = ev.Id,
                Title = ev.Title,
                Description = ev.Description,
                Date = ev.Date,
                Location = ev.Location,
                Attendees = new List<AttendeeDto>()
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEvent(int id, UpdateEventDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var ev = await _context.Events.FindAsync(id);
            if (ev == null) return NotFound();

            ev.Title = dto.Title;
            ev.Description = dto.Description;
            ev.Date = dto.Date;
            ev.Location = dto.Location;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var ev = await _context.Events.FindAsync(id);
            if (ev == null) return NotFound();

            _context.Events.Remove(ev);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost("{eventId}/attendees")]
        public async Task<IActionResult> Register(int eventId, RegisterAttendeeDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var ev = await _context.Events.FindAsync(eventId);
            if (ev == null) return NotFound();

            var attendee = new Attendee
            {
                Id = Guid.NewGuid().ToString(),
                Name = dto.Name,
                Email = dto.Email,
                EventId = eventId
            };

            _context.Attendees.Add(attendee);
            await _context.SaveChangesAsync();

            return StatusCode(201, new AttendeeDto
            {
                Id = attendee.Id,
                Name = attendee.Name,
                Email = attendee.Email
            });
        }

        [HttpDelete("{eventId}/attendees/{attendeeId}")]
        public async Task<IActionResult> Unregister(int eventId, string attendeeId)
        {
            var attendee = await _context.Attendees
                .FirstOrDefaultAsync(a => a.Id == attendeeId && a.EventId == eventId);

            if (attendee == null) return NotFound();

            _context.Attendees.Remove(attendee);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}