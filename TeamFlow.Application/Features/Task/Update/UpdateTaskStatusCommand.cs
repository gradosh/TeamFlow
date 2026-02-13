using MediatR;

public record UpdateTaskStatusCommand(
    Guid TaskId,
    TeamFlow.Domain.Enums.TaskStatus Status) : IRequest;
