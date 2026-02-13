using MediatR;

namespace TeamFlow.Application.Features.Projects.Create;

public record CreateProjectCommand(string Name, string? Description)
    : IRequest<Guid>;
