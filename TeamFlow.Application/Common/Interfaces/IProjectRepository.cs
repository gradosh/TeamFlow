using TeamFlow.Domain.Entities;

namespace TeamFlow.Application.Common.Interfaces;

public interface IProjectRepository
{
    Task AddAsync(Project project, CancellationToken cancellationToken);
    Task<List<Project>> GetByOwnerAsync(Guid ownerId);
    Task<Project?> GetByIdAsync(Guid id);
    Task RemoveAsync(Project project);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
