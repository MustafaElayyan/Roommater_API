using System.ComponentModel.DataAnnotations;

namespace Roommater_API.DTOs.Households;

public class HouseholdDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public Guid CreatedById { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<Guid> MemberIds { get; set; } = new();
}

public class CreateHouseholdDto
{
    [Required]
    [MaxLength(150)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string Code { get; set; } = string.Empty;

    [Required]
    public Guid CreatedById { get; set; }
}

public class UpdateHouseholdDto
{
    [Required]
    [MaxLength(150)]
    public string Name { get; set; } = string.Empty;
}

public class JoinHouseholdDto
{
    [Required]
    public Guid UserId { get; set; }

    [Required]
    [MaxLength(20)]
    public string Code { get; set; } = string.Empty;
}

public class LeaveHouseholdDto
{
    [Required]
    public Guid UserId { get; set; }
}
