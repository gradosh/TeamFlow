using AutoMapper;
using TeamFlow.API.Contracts.Projects;
using TeamFlow.Domain.Entities;
using TeamFlow.Contracts.Tasks;
using TeamFlow.Application.Features.Projects.Create;

namespace TeamFlow.API.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Project
        CreateMap<Project, CreateProjectResponse>();
        CreateMap<CreateProjectCommand, CreateProjectRequest>().ReverseMap();

        // Task
        CreateMap<TaskItem, TaskDto>();
        CreateMap<CreateTaskRequest, CreateTaskCommand>().ReverseMap();

        CreateMap<UpdateTaskStatusRequest, UpdateTaskStatusCommand>().ReverseMap();
        // Board
        CreateMap<BoardDto, BoardResponse>();
    }
}
