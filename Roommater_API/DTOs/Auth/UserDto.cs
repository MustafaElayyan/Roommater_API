namespace Roommater_API.DTOs.Auth;

public class UserDto
{
    public Guid Uid { get; set; }
    public string Email { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string PhotoUrl { get; set; } = string.Empty;
}
