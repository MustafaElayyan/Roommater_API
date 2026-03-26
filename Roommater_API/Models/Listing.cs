using System.ComponentModel.DataAnnotations;

namespace Roommater_API.Models;

public class Listing
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(2000)]
    public string Description { get; set; } = string.Empty;

    [MaxLength(255)]
    public string Location { get; set; } = string.Empty;

    public decimal Price { get; set; }

    [MaxLength(3000)]
    public string PhotoUrls { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Guid OwnerId { get; set; }
    public User? Owner { get; set; }
}
