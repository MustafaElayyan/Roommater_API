using System.ComponentModel.DataAnnotations;

namespace Roommater_API.DTOs.Grocery;

public class GroceryItemDto
{
    public Guid Id { get; set; }
    public Guid HouseholdId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Quantity { get; set; } = string.Empty;
    public Guid AddedById { get; set; }
    public bool IsPurchased { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateGroceryItemDto
{
    [Required]
    public Guid HouseholdId { get; set; }

    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(100)]
    public string Quantity { get; set; } = string.Empty;

    [Required]
    public Guid AddedById { get; set; }
}

public class UpdateGroceryItemDto
{
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(100)]
    public string Quantity { get; set; } = string.Empty;

    public bool IsPurchased { get; set; }
}
