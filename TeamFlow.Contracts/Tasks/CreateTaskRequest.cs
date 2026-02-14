using System;

namespace TeamFlow.Contracts.Tasks;

public record CreateTaskRequest(
    Guid ProjectId,
    string Title,
    string? Description);

    public record CreateTaskResponse(
    Guid Id,
    Guid ProjectId,
    string Title,
    string? Description,
    DateTime CreatedAt);