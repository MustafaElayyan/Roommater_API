using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Roommater_API.Data;
using Roommater_API.DTOs.Households;
using Roommater_API.Models;

namespace Roommater_API.Controllers;

[ApiController]
[Authorize]
[Route("api/households")]
public class HouseholdsController : ControllerBase
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IMapper _mapper;

    public HouseholdsController(ApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<HouseholdDto>>> GetHouseholds([FromQuery] int limit = 20, [FromQuery] int offset = 0)
    {
        limit = Math.Clamp(limit, 1, 100);
        offset = Math.Max(offset, 0);

        var households = await _dbContext.Households
            .Include(h => h.Members)
            .OrderByDescending(h => h.CreatedAt)
            .Skip(offset)
            .Take(limit)
            .AsNoTracking()
            .ToListAsync();

        return Ok(_mapper.Map<List<HouseholdDto>>(households));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<HouseholdDto>> GetHousehold(Guid id)
    {
        var household = await _dbContext.Households
            .Include(h => h.Members)
            .AsNoTracking()
            .FirstOrDefaultAsync(h => h.Id == id);

        if (household is null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<HouseholdDto>(household));
    }

    [HttpPost]
    public async Task<ActionResult<HouseholdDto>> CreateHousehold([FromBody] CreateHouseholdDto request)
    {
        var household = _mapper.Map<Household>(request);
        household.CreatedAt = DateTime.UtcNow;

        _dbContext.Households.Add(household);
        _dbContext.HouseholdMembers.Add(new HouseholdMember
        {
            HouseholdId = household.Id,
            UserId = household.CreatedById,
            JoinedAt = DateTime.UtcNow
        });
        await _dbContext.SaveChangesAsync();

        var saved = await _dbContext.Households
            .Include(h => h.Members)
            .AsNoTracking()
            .FirstAsync(h => h.Id == household.Id);

        return CreatedAtAction(nameof(GetHousehold), new { id = household.Id }, _mapper.Map<HouseholdDto>(saved));
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<HouseholdDto>> UpdateHousehold(Guid id, [FromBody] UpdateHouseholdDto request)
    {
        var household = await _dbContext.Households.Include(h => h.Members).FirstOrDefaultAsync(h => h.Id == id);
        if (household is null)
        {
            return NotFound();
        }

        household.Name = request.Name;
        await _dbContext.SaveChangesAsync();

        return Ok(_mapper.Map<HouseholdDto>(household));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteHousehold(Guid id)
    {
        var household = await _dbContext.Households.FirstOrDefaultAsync(h => h.Id == id);
        if (household is null)
        {
            return NotFound();
        }

        _dbContext.Households.Remove(household);
        await _dbContext.SaveChangesAsync();
        return NoContent();
    }

    [HttpPost("join")]
    public async Task<ActionResult<HouseholdDto>> JoinHousehold([FromBody] JoinHouseholdDto request)
    {
        var household = await _dbContext.Households
            .Include(h => h.Members)
            .FirstOrDefaultAsync(h => h.Code == request.Code);

        if (household is null)
        {
            return NotFound(new { message = "Household not found." });
        }

        var exists = household.Members.Any(m => m.UserId == request.UserId);
        if (!exists)
        {
            _dbContext.HouseholdMembers.Add(new HouseholdMember
            {
                HouseholdId = household.Id,
                UserId = request.UserId,
                JoinedAt = DateTime.UtcNow
            });
            await _dbContext.SaveChangesAsync();
            household = await _dbContext.Households.Include(h => h.Members).FirstAsync(h => h.Id == household.Id);
        }

        return Ok(_mapper.Map<HouseholdDto>(household));
    }

    [HttpPost("{id:guid}/leave")]
    public async Task<IActionResult> LeaveHousehold(Guid id, [FromBody] LeaveHouseholdDto request)
    {
        var member = await _dbContext.HouseholdMembers.FirstOrDefaultAsync(m => m.HouseholdId == id && m.UserId == request.UserId);
        if (member is null)
        {
            return NotFound();
        }

        _dbContext.HouseholdMembers.Remove(member);
        await _dbContext.SaveChangesAsync();
        return NoContent();
    }
}
