using System.ComponentModel.DataAnnotations;

namespace Roommater_API.Models;

public class Profile
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid UserId { get; set; }
    public User? User { get; set; }

    public int? Age { get; set; }

    [MaxLength(150)]
    public string Occupation { get; set; } = string.Empty;

    [MaxLength(255)]
    public string Location { get; set; } = string.Empty;

    [MaxLength(2000)]
    public string Bio { get; set; } = string.Empty;
}
