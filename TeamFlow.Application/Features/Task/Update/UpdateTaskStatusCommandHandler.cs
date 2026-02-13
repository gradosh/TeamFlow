using MediatR;
using TeamFlow.Application.Common.Exceptions;
using TeamFlow.Application.Common.Interfaces;

public class UpdateTaskStatusCommandHandler
    : IRequestHandler<UpdateTaskStatusCommand>
{
    private readonly ITaskRepository _taskRepo;
    private readonly IProjectRepository _projectRepo;
    private readonly ICurrentUserService _currentUser;

    public UpdateTaskStatusCommandHandler(
        ITaskRepository taskRepo,
        IProjectRepository projectRepo,
        ICurrentUserService currentUser)
    {
        _taskRepo = taskRepo;
        _projectRepo = projectRepo;
        _currentUser = currentUser;
    }

    public async Task Handle(
        UpdateTaskStatusCommand request,
        CancellationToken cancellationToken)
    {
        var task = await _taskRepo.GetByIdAsync(request.TaskId)
            ?? throw new NotFoundException("Task not found");

        var project = await _projectRepo.GetByIdAsync(task.ProjectId)
            ?? throw new NotFoundException("Project not found");

        if (project.OwnerId != _currentUser.UserId)
            throw new UnauthorizedException("Access denied");

        task.UpdateStatus(request.Status);

        await _taskRepo.SaveChangesAsync(cancellationToken);
    }
}
