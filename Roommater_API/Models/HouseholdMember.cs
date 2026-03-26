namespace Roommater_API.Models;

public class HouseholdMember
{
    public Guid HouseholdId { get; set; }
    public Household? Household { get; set; }

    public Guid UserId { get; set; }
    public User? User { get; set; }

    public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
}
