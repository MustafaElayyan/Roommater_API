using System.ComponentModel.DataAnnotations;

namespace Roommater_API.DTOs.Auth;

public class AuthRequestDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MinLength(8)]
    public string Password { get; set; } = string.Empty;
}
