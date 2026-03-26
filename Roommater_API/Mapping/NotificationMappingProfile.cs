using AutoMapper;
using Roommater_API.DTOs.Notifications;
using Roommater_API.Models;

namespace Roommater_API.Mapping;

public class NotificationMappingProfile : AutoMapper.Profile
{
    public NotificationMappingProfile()
    {
        CreateMap<Notification, NotificationDto>();
        CreateMap<CreateNotificationDto, Notification>();
    }
}
