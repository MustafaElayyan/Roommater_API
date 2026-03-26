using System.ComponentModel.DataAnnotations;

namespace Roommater_API.Models;

public class GroceryItem
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid HouseholdId { get; set; }
    public Household? Household { get; set; }

    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(100)]
    public string Quantity { get; set; } = string.Empty;

    public Guid AddedById { get; set; }
    public User? AddedBy { get; set; }

    public bool IsPurchased { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
