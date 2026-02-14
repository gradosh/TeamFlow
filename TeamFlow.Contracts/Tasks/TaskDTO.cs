using System;

namespace TeamFlow.Contracts.Tasks;

public record TaskDto(
    Guid Id,
    string Title,
    string? Description,
    TaskStatus Status);

public record BoardResponse(
    List<TaskDto> Todo,
    List<TaskDto> InProgress,
    List<TaskDto> Done);
