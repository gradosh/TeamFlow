namespace TeamFlow.API.Contracts.Projects;

public record CreateProjectRequest(
    string Name,
    string? Description);

public record CreateProjectResponse(
    Guid ProjectId,
    string Name,
    string? Description,
    DateTime CreatedAt);