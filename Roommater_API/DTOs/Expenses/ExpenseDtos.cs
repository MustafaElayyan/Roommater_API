using System.ComponentModel.DataAnnotations;

namespace Roommater_API.DTOs.Expenses;

public class ExpenseDto
{
    public Guid Id { get; set; }
    public Guid HouseholdId { get; set; }
    public string Title { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public Guid PaidById { get; set; }
    public string SplitAmong { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

public class CreateExpenseDto
{
    [Required]
    public Guid HouseholdId { get; set; }

    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    public decimal Amount { get; set; }

    [Required]
    public Guid PaidById { get; set; }

    [MaxLength(2000)]
    public string SplitAmong { get; set; } = string.Empty;

    [MaxLength(255)]
    public string Category { get; set; } = string.Empty;
}

public class UpdateExpenseDto
{
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    public decimal Amount { get; set; }

    [Required]
    public Guid PaidById { get; set; }

    [MaxLength(2000)]
    public string SplitAmong { get; set; } = string.Empty;

    [MaxLength(255)]
    public string Category { get; set; } = string.Empty;
}
