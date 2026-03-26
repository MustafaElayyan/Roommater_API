using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Roommater_API.Data;
using Roommater_API.DTOs.Tasks;
using TaskEntity = Roommater_API.Models.Task;

namespace Roommater_API.Controllers;

[ApiController]
[Authorize]
[Route("api/tasks")]
public class TasksController : ControllerBase
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IMapper _mapper;

    public TasksController(ApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TaskDto>>> GetTasks([FromQuery] Guid householdId, [FromQuery] int limit = 20, [FromQuery] int offset = 0)
    {
        limit = Math.Clamp(limit, 1, 100);
        offset = Math.Max(offset, 0);

        var query = _dbContext.Tasks.AsNoTracking().OrderByDescending(t => t.CreatedAt).AsQueryable();
        if (householdId != Guid.Empty)
        {
            query = query.Where(t => t.HouseholdId == householdId);
        }

        var tasks = await query.Skip(offset).Take(limit).ToListAsync();
        return Ok(_mapper.Map<List<TaskDto>>(tasks));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<TaskDto>> GetTask(Guid id)
    {
        var task = await _dbContext.Tasks.AsNoTracking().FirstOrDefaultAsync(t => t.Id == id);
        if (task is null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<TaskDto>(task));
    }

    [HttpPost]
    public async Task<ActionResult<TaskDto>> CreateTask([FromBody] CreateTaskDto request)
    {
        var task = _mapper.Map<TaskEntity>(request);
        task.CreatedAt = DateTime.UtcNow;

        _dbContext.Tasks.Add(task);
        await _dbContext.SaveChangesAsync();

        return CreatedAtAction(nameof(GetTask), new { id = task.Id }, _mapper.Map<TaskDto>(task));
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<TaskDto>> UpdateTask(Guid id, [FromBody] UpdateTaskDto request)
    {
        var task = await _dbContext.Tasks.FirstOrDefaultAsync(t => t.Id == id);
        if (task is null)
        {
            return NotFound();
        }

        task.Title = request.Title;
        task.Description = request.Description;
        task.AssignedToId = request.AssignedToId;
        task.DueDate = request.DueDate;
        task.IsCompleted = request.IsCompleted;

        await _dbContext.SaveChangesAsync();
        return Ok(_mapper.Map<TaskDto>(task));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteTask(Guid id)
    {
        var task = await _dbContext.Tasks.FirstOrDefaultAsync(t => t.Id == id);
        if (task is null)
        {
            return NotFound();
        }

        _dbContext.Tasks.Remove(task);
        await _dbContext.SaveChangesAsync();
        return NoContent();
    }
}
