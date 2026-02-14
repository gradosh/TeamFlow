using System;

namespace TeamFlow.Contracts.Tasks;

public record UpdateTaskStatusRequest(
    Guid TaskId,
    TaskStatus Status);
