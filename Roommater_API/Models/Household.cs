using System.ComponentModel.DataAnnotations;

namespace Roommater_API.Models;

public class Household
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    [MaxLength(150)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string Code { get; set; } = string.Empty;

    public Guid CreatedById { get; set; }
    public User? CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<HouseholdMember> Members { get; set; } = new List<HouseholdMember>();
    public ICollection<Task> Tasks { get; set; } = new List<Task>();
    public ICollection<Expense> Expenses { get; set; } = new List<Expense>();
    public ICollection<GroceryItem> GroceryItems { get; set; } = new List<GroceryItem>();
    public ICollection<Event> Events { get; set; } = new List<Event>();
}
