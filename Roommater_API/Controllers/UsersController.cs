using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Roommater_API.Data;
using Roommater_API.DTOs.Users;
using Roommater_API.Models;

namespace Roommater_API.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IMapper _mapper;

    public UsersController(ApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    [Authorize]
    [HttpGet("{uid:guid}")]
    public async Task<ActionResult<UserProfileDto>> GetUser(Guid uid)
    {
        var user = await _dbContext.Users
            .Include(u => u.Profile)
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == uid);

        if (user is null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<UserProfileDto>(user));
    }

    [Authorize]
    [HttpPut("{uid:guid}")]
    public async Task<ActionResult<UserProfileDto>> UpdateUser(Guid uid, [FromBody] UpdateUserProfileDto request)
    {
        var userIdValue = User.FindFirst("sub")?.Value;
        if (!Guid.TryParse(userIdValue, out var userId) || userId != uid)
        {
            return Forbid();
        }

        var user = await _dbContext.Users.Include(u => u.Profile).FirstOrDefaultAsync(u => u.Id == uid);
        if (user is null)
        {
            return NotFound();
        }

        user.DisplayName = request.DisplayName;
        user.PhotoUrl = request.PhotoUrl;

        if (user.Profile is null)
        {
            user.Profile = new Roommater_API.Models.Profile { UserId = uid };
        }

        user.Profile.Age = request.Age;
        user.Profile.Occupation = request.Occupation;
        user.Profile.Location = request.Location;
        user.Profile.Bio = request.Bio;

        await _dbContext.SaveChangesAsync();
        return Ok(_mapper.Map<UserProfileDto>(user));
    }
}
