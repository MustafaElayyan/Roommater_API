using System.ComponentModel.DataAnnotations;

namespace Roommater_API.DTOs.Listings;

public class ListingDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public List<string> PhotoUrls { get; set; } = new();
    public Guid OwnerId { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateListingDto
{
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(2000)]
    public string Description { get; set; } = string.Empty;

    [MaxLength(255)]
    public string Location { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public List<string> PhotoUrls { get; set; } = new();
}
