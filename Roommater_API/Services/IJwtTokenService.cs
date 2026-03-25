using Roommater_API.Models;

namespace Roommater_API.Services;

public interface IJwtTokenService
{
    string GenerateToken(User user);
}
