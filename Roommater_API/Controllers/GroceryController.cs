using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Roommater_API.Data;
using Roommater_API.DTOs.Grocery;
using Roommater_API.Models;

namespace Roommater_API.Controllers;

[ApiController]
[Authorize]
[Route("api/grocery")]
public class GroceryController : ControllerBase
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IMapper _mapper;

    public GroceryController(ApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<GroceryItemDto>>> GetItems([FromQuery] Guid householdId, [FromQuery] int limit = 20, [FromQuery] int offset = 0)
    {
        limit = Math.Clamp(limit, 1, 100);
        offset = Math.Max(offset, 0);

        var query = _dbContext.GroceryItems.AsNoTracking().OrderByDescending(g => g.CreatedAt).AsQueryable();
        if (householdId != Guid.Empty)
        {
            query = query.Where(g => g.HouseholdId == householdId);
        }

        var items = await query.Skip(offset).Take(limit).ToListAsync();
        return Ok(_mapper.Map<List<GroceryItemDto>>(items));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<GroceryItemDto>> GetItem(Guid id)
    {
        var item = await _dbContext.GroceryItems.AsNoTracking().FirstOrDefaultAsync(g => g.Id == id);
        if (item is null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<GroceryItemDto>(item));
    }

    [HttpPost]
    public async Task<ActionResult<GroceryItemDto>> CreateItem([FromBody] CreateGroceryItemDto request)
    {
        var item = _mapper.Map<GroceryItem>(request);
        item.CreatedAt = DateTime.UtcNow;

        _dbContext.GroceryItems.Add(item);
        await _dbContext.SaveChangesAsync();

        return CreatedAtAction(nameof(GetItem), new { id = item.Id }, _mapper.Map<GroceryItemDto>(item));
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<GroceryItemDto>> UpdateItem(Guid id, [FromBody] UpdateGroceryItemDto request)
    {
        var item = await _dbContext.GroceryItems.FirstOrDefaultAsync(g => g.Id == id);
        if (item is null)
        {
            return NotFound();
        }

        item.Name = request.Name;
        item.Quantity = request.Quantity;
        item.IsPurchased = request.IsPurchased;

        await _dbContext.SaveChangesAsync();
        return Ok(_mapper.Map<GroceryItemDto>(item));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteItem(Guid id)
    {
        var item = await _dbContext.GroceryItems.FirstOrDefaultAsync(g => g.Id == id);
        if (item is null)
        {
            return NotFound();
        }

        _dbContext.GroceryItems.Remove(item);
        await _dbContext.SaveChangesAsync();
        return NoContent();
    }
}
