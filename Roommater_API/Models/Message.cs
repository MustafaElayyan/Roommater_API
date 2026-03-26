using System.ComponentModel.DataAnnotations;

namespace Roommater_API.Models;

public class Message
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid ChatId { get; set; }
    public Chat? Chat { get; set; }

    public Guid SenderId { get; set; }
    public User? Sender { get; set; }

    [Required]
    [MaxLength(2000)]
    public string Content { get; set; } = string.Empty;

    public DateTime SentAt { get; set; } = DateTime.UtcNow;
}
