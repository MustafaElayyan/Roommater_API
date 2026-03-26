using AutoMapper;
using Roommater_API.DTOs.Households;
using Roommater_API.Models;

namespace Roommater_API.Mapping;

public class HouseholdMappingProfile : AutoMapper.Profile
{
    public HouseholdMappingProfile()
    {
        CreateMap<Household, HouseholdDto>()
            .ForMember(dest => dest.MemberIds, opt => opt.MapFrom(src => src.Members.Select(m => m.UserId).ToList()));

        CreateMap<CreateHouseholdDto, Household>();
    }
}
