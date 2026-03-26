using System.ComponentModel.DataAnnotations;

namespace Roommater_API.Models;

public class Event
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid HouseholdId { get; set; }
    public Household? Household { get; set; }

    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    public DateTime Date { get; set; }

    [MaxLength(2000)]
    public string Description { get; set; } = string.Empty;

    public Guid CreatedById { get; set; }
    public User? CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
