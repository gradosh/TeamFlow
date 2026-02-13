using TeamFlow.Domain.Entities;

public record BoardDto(
    List<TaskItem> Todo,
    List<TaskItem> InProgress,
    List<TaskItem> Done);
