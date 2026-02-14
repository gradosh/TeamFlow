using TeamFlow.Domain.Enums;
using TeamFlow.Domain.Events;

namespace TeamFlow.Domain.Entities;

public class TaskItem  : BaseEntity
{
    public Guid Id { get; private set; }
    public string Title { get; private set; } = null!;
    public string? Description { get; private set; }
    public TeamFlow.Domain.Enums.TaskStatus Status { get; private set; }
    public Guid ProjectId { get; private set; }
    public Guid? AssignedUserId { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private TaskItem() { }

    public TaskItem(string title, string? description, Guid projectId)
    {
        Id = Guid.NewGuid();
        Title = title;
        Description = description;
        ProjectId = projectId;
        Status = Enums.TaskStatus.Todo;
        CreatedAt = DateTime.UtcNow;
    }

    public void UpdateStatus(Enums.TaskStatus status,Guid userId)
    {
        Status = status;
        AddDomainEvent(new TaskStatusUpdatedEvent(ProjectId, userId));
    }
}
