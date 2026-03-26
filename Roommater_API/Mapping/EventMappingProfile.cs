using AutoMapper;
using Roommater_API.DTOs.Events;
using Roommater_API.Models;

namespace Roommater_API.Mapping;

public class EventMappingProfile : AutoMapper.Profile
{
    public EventMappingProfile()
    {
        CreateMap<Event, EventDto>();
        CreateMap<CreateEventDto, Event>();
    }
}
