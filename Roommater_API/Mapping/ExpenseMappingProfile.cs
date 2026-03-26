using AutoMapper;
using Roommater_API.DTOs.Expenses;
using Roommater_API.Models;

namespace Roommater_API.Mapping;

public class ExpenseMappingProfile : AutoMapper.Profile
{
    public ExpenseMappingProfile()
    {
        CreateMap<Expense, ExpenseDto>();
        CreateMap<CreateExpenseDto, Expense>();
    }
}
