using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Roommater_API.Data;
using Roommater_API.DTOs.Chats;
using Roommater_API.Models;

namespace Roommater_API.Controllers;

[ApiController]
[Authorize]
[Route("api/chats")]
public class ChatsController : ControllerBase
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IMapper _mapper;

    public ChatsController(ApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ChatDto>>> GetChats([FromQuery] Guid userId, [FromQuery] int limit = 20, [FromQuery] int offset = 0)
    {
        limit = Math.Clamp(limit, 1, 100);
        offset = Math.Max(offset, 0);

        var chats = await _dbContext.Chats
            .Include(c => c.Participants)
            .Where(c => c.Participants.Any(p => p.UserId == userId))
            .OrderByDescending(c => c.LastMessageAt ?? c.CreatedAt)
            .Skip(offset)
            .Take(limit)
            .AsNoTracking()
            .ToListAsync();

        return Ok(_mapper.Map<List<ChatDto>>(chats));
    }

    [HttpGet("{chatId:guid}/messages")]
    public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessages(Guid chatId, [FromQuery] int limit = 50, [FromQuery] int offset = 0)
    {
        limit = Math.Clamp(limit, 1, 200);
        offset = Math.Max(offset, 0);

        var chatExists = await _dbContext.Chats.AnyAsync(c => c.Id == chatId);
        if (!chatExists)
        {
            return NotFound();
        }

        var messages = await _dbContext.Messages
            .Where(m => m.ChatId == chatId)
            .OrderBy(m => m.SentAt)
            .Skip(offset)
            .Take(limit)
            .AsNoTracking()
            .ToListAsync();

        return Ok(_mapper.Map<List<MessageDto>>(messages));
    }

    [HttpPost("{chatId:guid}/messages")]
    public async Task<ActionResult<MessageDto>> CreateMessage(Guid chatId, [FromBody] CreateMessageDto request)
    {
        var chat = await _dbContext.Chats.FirstOrDefaultAsync(c => c.Id == chatId);
        if (chat is null)
        {
            return NotFound();
        }

        var isParticipant = await _dbContext.ChatParticipants.AnyAsync(cp => cp.ChatId == chatId && cp.UserId == request.SenderId);
        if (!isParticipant)
        {
            return Forbid();
        }

        var message = new Message
        {
            ChatId = chatId,
            SenderId = request.SenderId,
            Content = request.Content,
            SentAt = DateTime.UtcNow
        };

        chat.LastMessage = request.Content;
        chat.LastMessageAt = message.SentAt;

        _dbContext.Messages.Add(message);
        await _dbContext.SaveChangesAsync();

        return CreatedAtAction(nameof(GetMessages), new { chatId }, _mapper.Map<MessageDto>(message));
    }
}
