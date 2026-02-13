using MediatR;

public record CreateTaskCommand(
    Guid ProjectId,
    string Title,
    string? Description) : IRequest<Guid>;
