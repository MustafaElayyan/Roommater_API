using System.ComponentModel.DataAnnotations;

namespace Roommater_API.DTOs.Tasks;

public class TaskDto
{
    public Guid Id { get; set; }
    public Guid HouseholdId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Guid? AssignedToId { get; set; }
    public DateTime? DueDate { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateTaskDto
{
    [Required]
    public Guid HouseholdId { get; set; }

    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string Description { get; set; } = string.Empty;

    public Guid? AssignedToId { get; set; }
    public DateTime? DueDate { get; set; }
}

public class UpdateTaskDto
{
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string Description { get; set; } = string.Empty;

    public Guid? AssignedToId { get; set; }
    public DateTime? DueDate { get; set; }
    public bool IsCompleted { get; set; }
}
