using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Roommater_API.Data;
using Roommater_API.DTOs.Events;
using Roommater_API.Models;

namespace Roommater_API.Controllers;

[ApiController]
[Authorize]
[Route("api/events")]
public class EventsController : ControllerBase
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IMapper _mapper;

    public EventsController(ApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<EventDto>>> GetEvents([FromQuery] Guid householdId, [FromQuery] int limit = 20, [FromQuery] int offset = 0)
    {
        limit = Math.Clamp(limit, 1, 100);
        offset = Math.Max(offset, 0);

        var query = _dbContext.Events.AsNoTracking().OrderBy(e => e.Date).AsQueryable();
        if (householdId != Guid.Empty)
        {
            query = query.Where(e => e.HouseholdId == householdId);
        }

        var events = await query.Skip(offset).Take(limit).ToListAsync();
        return Ok(_mapper.Map<List<EventDto>>(events));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<EventDto>> GetEvent(Guid id)
    {
        var evt = await _dbContext.Events.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);
        if (evt is null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<EventDto>(evt));
    }

    [HttpPost]
    public async Task<ActionResult<EventDto>> CreateEvent([FromBody] CreateEventDto request)
    {
        var evt = _mapper.Map<Event>(request);
        evt.CreatedAt = DateTime.UtcNow;

        _dbContext.Events.Add(evt);
        await _dbContext.SaveChangesAsync();

        return CreatedAtAction(nameof(GetEvent), new { id = evt.Id }, _mapper.Map<EventDto>(evt));
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<EventDto>> UpdateEvent(Guid id, [FromBody] UpdateEventDto request)
    {
        var evt = await _dbContext.Events.FirstOrDefaultAsync(e => e.Id == id);
        if (evt is null)
        {
            return NotFound();
        }

        evt.Title = request.Title;
        evt.Date = request.Date;
        evt.Description = request.Description;

        await _dbContext.SaveChangesAsync();
        return Ok(_mapper.Map<EventDto>(evt));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteEvent(Guid id)
    {
        var evt = await _dbContext.Events.FirstOrDefaultAsync(e => e.Id == id);
        if (evt is null)
        {
            return NotFound();
        }

        _dbContext.Events.Remove(evt);
        await _dbContext.SaveChangesAsync();
        return NoContent();
    }
}
