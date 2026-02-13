using MediatR;
using TeamFlow.Application.Common.Exceptions;
using TeamFlow.Application.Common.Interfaces;

public class CreateTaskCommandHandler 
    : IRequestHandler<CreateTaskCommand, Guid>
{
    private readonly ITaskRepository _taskRepo;
    private readonly IProjectRepository _projectRepo;
    private readonly ICurrentUserService _currentUser;
    private readonly ICacheService _cache;

    public CreateTaskCommandHandler(
        ITaskRepository taskRepo,
        IProjectRepository projectRepo,
        ICurrentUserService currentUser,
        ICacheService cache)
    {
        _taskRepo = taskRepo;
        _projectRepo = projectRepo;
        _currentUser = currentUser;
        _cache  = cache;
    }

    public async Task<Guid> Handle(
        CreateTaskCommand request,
        CancellationToken cancellationToken)
    {
        var project = await _projectRepo.GetByIdAsync(request.ProjectId)
            ?? throw new NotFoundException("Project not found");

        if (project.OwnerId != _currentUser.UserId)
            throw new UnauthorizedException("Access denied");

        var task = new TeamFlow.Domain.Entities.TaskItem(
            request.Title,
            request.Description,
            request.ProjectId);

        await _taskRepo.AddAsync(task, cancellationToken);
        await _taskRepo.SaveChangesAsync(cancellationToken);

        await _cache.RemoveAsync($"board:{_currentUser.UserId}:{request.ProjectId}");
        
        return task.Id;
    }
}
