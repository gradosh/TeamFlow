using System;
using MediatR;
using TeamFlow.Application.Common.Events;
using TeamFlow.Domain.Events;

namespace TeamFlow.Application.Features.Task.Update;

public class TaskStatusUpdatedEventHandler
    : INotificationHandler<DomainEventNotification<TaskStatusUpdatedEvent>>
{
    public async System.Threading.Tasks.Task Handle(DomainEventNotification<TaskStatusUpdatedEvent> notification, CancellationToken cancellationToken)
    {
         var cacheKey =
            $"board:{notification.DomainEvent.UserId}:{notification.DomainEvent.ProjectId}";

        await _cache.RemoveAsync(cacheKey);
    }


    private readonly ICacheService _cache;

    public TaskStatusUpdatedEventHandler(ICacheService cache)
    {
        _cache = cache;
    }
}
