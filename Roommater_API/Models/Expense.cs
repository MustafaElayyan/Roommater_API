using System.ComponentModel.DataAnnotations;

namespace Roommater_API.Models;

public class Expense
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid HouseholdId { get; set; }
    public Household? Household { get; set; }

    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    public decimal Amount { get; set; }

    public Guid PaidById { get; set; }
    public User? PaidBy { get; set; }

    [MaxLength(255)]
    public string Category { get; set; } = string.Empty;

    [MaxLength(2000)]
    public string SplitAmong { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
