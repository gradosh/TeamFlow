using System;

namespace TeamFlow.Domain.Events;

public class TaskStatusUpdatedEvent : DomainEvent
{
    public Guid ProjectId { get; }
    public Guid UserId { get; }

    public TaskStatusUpdatedEvent(Guid projectId, Guid userId)
    {
        ProjectId = projectId;
        UserId = userId;
    }
}