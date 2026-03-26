using System.ComponentModel.DataAnnotations;

namespace Roommater_API.DTOs.Chats;

public class ChatDto
{
    public Guid Id { get; set; }
    public List<Guid> ParticipantIds { get; set; } = new();
    public string LastMessage { get; set; } = string.Empty;
    public DateTime? LastMessageAt { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class MessageDto
{
    public Guid Id { get; set; }
    public Guid ChatId { get; set; }
    public Guid SenderId { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTime SentAt { get; set; }
}

public class CreateMessageDto
{
    [Required]
    public Guid SenderId { get; set; }

    [Required]
    [MaxLength(2000)]
    public string Content { get; set; } = string.Empty;
}
