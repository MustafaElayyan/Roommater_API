using System.ComponentModel.DataAnnotations;

namespace Roommater_API.DTOs.Events;

public class EventDto
{
    public Guid Id { get; set; }
    public Guid HouseholdId { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string Description { get; set; } = string.Empty;
    public Guid CreatedById { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateEventDto
{
    [Required]
    public Guid HouseholdId { get; set; }

    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    public DateTime Date { get; set; }

    [MaxLength(2000)]
    public string Description { get; set; } = string.Empty;

    [Required]
    public Guid CreatedById { get; set; }
}

public class UpdateEventDto
{
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    public DateTime Date { get; set; }

    [MaxLength(2000)]
    public string Description { get; set; } = string.Empty;
}
