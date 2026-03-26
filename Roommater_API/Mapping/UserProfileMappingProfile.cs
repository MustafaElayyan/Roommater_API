using AutoMapper;
using Roommater_API.DTOs.Users;
using Roommater_API.Models;

namespace Roommater_API.Mapping;

public class UserProfileMappingProfile : AutoMapper.Profile
{
    public UserProfileMappingProfile()
    {
        CreateMap<User, UserProfileDto>()
            .ForMember(dest => dest.Uid, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.Profile != null ? src.Profile.Age : null))
            .ForMember(dest => dest.Occupation, opt => opt.MapFrom(src => src.Profile != null ? src.Profile.Occupation : string.Empty))
            .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.Profile != null ? src.Profile.Location : string.Empty))
            .ForMember(dest => dest.Bio, opt => opt.MapFrom(src => src.Profile != null ? src.Profile.Bio : string.Empty));
    }
}
