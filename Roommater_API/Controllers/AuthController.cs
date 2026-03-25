using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Roommater_API.DTOs.Auth;
using Roommater_API.Services;

namespace Roommater_API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IMapper _mapper;

    public AuthController(IUserService userService, IJwtTokenService jwtTokenService, IMapper mapper)
    {
        _userService = userService;
        _jwtTokenService = jwtTokenService;
        _mapper = mapper;
    }

    [HttpPost("signup")]
    public ActionResult<AuthResponseDto> SignUp([FromBody] AuthRequestDto request)
    {
        var email = _userService.NormalizeEmail(request.Email);
        if (_userService.GetByEmail(email) is not null)
        {
            return Conflict(new { message = "Email already in use." });
        }

        var user = _userService.Create(email, request.Password);
        var token = _jwtTokenService.GenerateToken(user);

        return Ok(new AuthResponseDto { Token = token });
    }

    [HttpPost("login")]
    public ActionResult<AuthResponseDto> Login([FromBody] AuthRequestDto request)
    {
        var email = _userService.NormalizeEmail(request.Email);
        var user = _userService.GetByEmail(email);

        if (user is null || !_userService.VerifyPassword(user, request.Password))
        {
            return Unauthorized(new { message = "Invalid email or password." });
        }

        var token = _jwtTokenService.GenerateToken(user);
        return Ok(new AuthResponseDto { Token = token });
    }

    [Authorize]
    [HttpGet("me")]
    public ActionResult<UserDto> Me()
    {
        var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdValue, out var userId))
        {
            return Unauthorized();
        }

        var user = _userService.GetById(userId);
        if (user is null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<UserDto>(user));
    }
}
