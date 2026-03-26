using AutoMapper;
using Roommater_API.DTOs.Grocery;
using Roommater_API.Models;

namespace Roommater_API.Mapping;

public class GroceryMappingProfile : AutoMapper.Profile
{
    public GroceryMappingProfile()
    {
        CreateMap<GroceryItem, GroceryItemDto>();
        CreateMap<CreateGroceryItemDto, GroceryItem>();
    }
}
