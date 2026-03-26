using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Roommater_API.Data;
using Roommater_API.DTOs.Expenses;
using Roommater_API.Models;

namespace Roommater_API.Controllers;

[ApiController]
[Authorize]
[Route("api/expenses")]
public class ExpensesController : ControllerBase
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IMapper _mapper;

    public ExpensesController(ApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ExpenseDto>>> GetExpenses([FromQuery] Guid householdId, [FromQuery] int limit = 20, [FromQuery] int offset = 0)
    {
        limit = Math.Clamp(limit, 1, 100);
        offset = Math.Max(offset, 0);

        var query = _dbContext.Expenses.AsNoTracking().OrderByDescending(e => e.CreatedAt).AsQueryable();
        if (householdId != Guid.Empty)
        {
            query = query.Where(e => e.HouseholdId == householdId);
        }

        var expenses = await query.Skip(offset).Take(limit).ToListAsync();
        return Ok(_mapper.Map<List<ExpenseDto>>(expenses));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ExpenseDto>> GetExpense(Guid id)
    {
        var expense = await _dbContext.Expenses.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);
        if (expense is null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<ExpenseDto>(expense));
    }

    [HttpPost]
    public async Task<ActionResult<ExpenseDto>> CreateExpense([FromBody] CreateExpenseDto request)
    {
        var expense = _mapper.Map<Expense>(request);
        expense.CreatedAt = DateTime.UtcNow;

        _dbContext.Expenses.Add(expense);
        await _dbContext.SaveChangesAsync();

        return CreatedAtAction(nameof(GetExpense), new { id = expense.Id }, _mapper.Map<ExpenseDto>(expense));
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<ExpenseDto>> UpdateExpense(Guid id, [FromBody] UpdateExpenseDto request)
    {
        var expense = await _dbContext.Expenses.FirstOrDefaultAsync(e => e.Id == id);
        if (expense is null)
        {
            return NotFound();
        }

        expense.Title = request.Title;
        expense.Amount = request.Amount;
        expense.PaidById = request.PaidById;
        expense.SplitAmong = request.SplitAmong;
        expense.Category = request.Category;

        await _dbContext.SaveChangesAsync();
        return Ok(_mapper.Map<ExpenseDto>(expense));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteExpense(Guid id)
    {
        var expense = await _dbContext.Expenses.FirstOrDefaultAsync(e => e.Id == id);
        if (expense is null)
        {
            return NotFound();
        }

        _dbContext.Expenses.Remove(expense);
        await _dbContext.SaveChangesAsync();
        return NoContent();
    }
}
