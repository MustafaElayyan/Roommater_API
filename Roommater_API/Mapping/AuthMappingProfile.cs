using AutoMapper;
using Roommater_API.DTOs.Auth;
using Roommater_API.Models;

namespace Roommater_API.Mapping;

public class AuthMappingProfile : AutoMapper.Profile
{
    public AuthMappingProfile()
    {
        CreateMap<User, UserDto>()
            .ForMember(dest => dest.Uid, opt => opt.MapFrom(src => src.Id));
    }
}
