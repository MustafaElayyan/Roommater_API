using AutoMapper;
using Roommater_API.DTOs.Chats;
using Roommater_API.Models;

namespace Roommater_API.Mapping;

public class ChatMappingProfile : AutoMapper.Profile
{
    public ChatMappingProfile()
    {
        CreateMap<Chat, ChatDto>()
            .ForMember(dest => dest.ParticipantIds, opt => opt.MapFrom(src => src.Participants.Select(p => p.UserId).ToList()));

        CreateMap<Message, MessageDto>();
    }
}
