using AutoMapper;
using Roommater_API.DTOs.Tasks;
using TaskEntity = Roommater_API.Models.Task;

namespace Roommater_API.Mapping;

public class TaskMappingProfile : AutoMapper.Profile
{
    public TaskMappingProfile()
    {
        CreateMap<TaskEntity, TaskDto>();
        CreateMap<CreateTaskDto, TaskEntity>();
    }
}
