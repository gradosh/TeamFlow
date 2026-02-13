using TeamFlow.Domain.Entities;

namespace TeamFlow.Application.Common.Interfaces;

public interface ITaskRepository
{
    Task AddAsync(TaskItem task, CancellationToken cancellationToken);
    Task<TaskItem?> GetByIdAsync(Guid id);
    Task<List<TaskItem>> GetByProjectAsync(Guid projectId);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
