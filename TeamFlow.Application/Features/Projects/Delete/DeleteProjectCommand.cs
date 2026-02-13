using MediatR;

public record DeleteProjectCommand(Guid ProjectId) : IRequest;
