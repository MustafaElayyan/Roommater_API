using AutoMapper;
using Roommater_API.DTOs.Listings;
using Roommater_API.Models;

namespace Roommater_API.Mapping;

public class ListingMappingProfile : AutoMapper.Profile
{
    public ListingMappingProfile()
    {
        CreateMap<Listing, ListingDto>()
            .ForMember(dest => dest.PhotoUrls, opt => opt.MapFrom(src => string.IsNullOrWhiteSpace(src.PhotoUrls)
                ? new List<string>()
                : src.PhotoUrls.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList()));

        CreateMap<CreateListingDto, Listing>()
            .ForMember(dest => dest.PhotoUrls, opt => opt.MapFrom(src => string.Join(',', src.PhotoUrls)));
    }
}
