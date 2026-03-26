using System.ComponentModel.DataAnnotations;

namespace Roommater_API.Models;

public class Task
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid HouseholdId { get; set; }
    public Household? Household { get; set; }

    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string Description { get; set; } = string.Empty;

    public Guid? AssignedToId { get; set; }
    public User? AssignedTo { get; set; }

    public DateTime? DueDate { get; set; }

    public bool IsCompleted { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
