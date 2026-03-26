using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Roommater_API.Data;
using Roommater_API.DTOs.Listings;
using Roommater_API.Models;

namespace Roommater_API.Controllers;

[ApiController]
[Route("api/listings")]
public class ListingsController : ControllerBase
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IMapper _mapper;

    public ListingsController(ApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<ListingDto>>> GetListings([FromQuery] int limit = 20, [FromQuery] int offset = 0)
    {
        limit = Math.Clamp(limit, 1, 100);
        offset = Math.Max(offset, 0);

        var listings = await _dbContext.Listings
            .AsNoTracking()
            .OrderByDescending(l => l.CreatedAt)
            .Skip(offset)
            .Take(limit)
            .ToListAsync();

        return Ok(_mapper.Map<List<ListingDto>>(listings));
    }

    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    public async Task<ActionResult<ListingDto>> GetListing(Guid id)
    {
        var listing = await _dbContext.Listings.AsNoTracking().FirstOrDefaultAsync(l => l.Id == id);
        if (listing is null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<ListingDto>(listing));
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<ListingDto>> CreateListing([FromBody] CreateListingDto request)
    {
        var userIdValue = User.FindFirst("sub")?.Value;
        if (!Guid.TryParse(userIdValue, out var userId))
        {
            return Unauthorized();
        }

        var listing = _mapper.Map<Listing>(request);
        listing.OwnerId = userId;
        listing.CreatedAt = DateTime.UtcNow;

        _dbContext.Listings.Add(listing);
        await _dbContext.SaveChangesAsync();

        var result = _mapper.Map<ListingDto>(listing);
        return CreatedAtAction(nameof(GetListing), new { id = listing.Id }, result);
    }

    [Authorize]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteListing(Guid id)
    {
        var userIdValue = User.FindFirst("sub")?.Value;
        if (!Guid.TryParse(userIdValue, out var userId))
        {
            return Unauthorized();
        }

        var listing = await _dbContext.Listings.FirstOrDefaultAsync(l => l.Id == id);
        if (listing is null)
        {
            return NotFound();
        }

        if (listing.OwnerId != userId)
        {
            return Forbid();
        }

        _dbContext.Listings.Remove(listing);
        await _dbContext.SaveChangesAsync();
        return NoContent();
    }
}
