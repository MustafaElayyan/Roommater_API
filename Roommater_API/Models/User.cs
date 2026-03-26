using System.ComponentModel.DataAnnotations;

namespace Roommater_API.Models;

public class User
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    [MaxLength(255)]
    public string Email { get; set; } = string.Empty;

    [MaxLength(100)]
    public string DisplayName { get; set; } = string.Empty;

    [MaxLength(500)]
    public string PhotoUrl { get; set; } = string.Empty;

    [MaxLength(255)]
    public string PasswordHash { get; set; } = string.Empty;

    public Profile? Profile { get; set; }
    public ICollection<Listing> Listings { get; set; } = new List<Listing>();
    public ICollection<ChatParticipant> ChatParticipants { get; set; } = new List<ChatParticipant>();
    public ICollection<Message> Messages { get; set; } = new List<Message>();
    public ICollection<Household> CreatedHouseholds { get; set; } = new List<Household>();
    public ICollection<HouseholdMember> HouseholdMemberships { get; set; } = new List<HouseholdMember>();
    public ICollection<Task> AssignedTasks { get; set; } = new List<Task>();
    public ICollection<Expense> PaidExpenses { get; set; } = new List<Expense>();
    public ICollection<GroceryItem> AddedGroceryItems { get; set; } = new List<GroceryItem>();
    public ICollection<Event> CreatedEvents { get; set; } = new List<Event>();
    public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
}
