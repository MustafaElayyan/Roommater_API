using System.ComponentModel.DataAnnotations;

namespace Roommater_API.Models;

public class Chat
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [MaxLength(1000)]
    public string LastMessage { get; set; } = string.Empty;

    public DateTime? LastMessageAt { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<ChatParticipant> Participants { get; set; } = new List<ChatParticipant>();
    public ICollection<Message> Messages { get; set; } = new List<Message>();
}
