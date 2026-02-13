using MediatR;
using TeamFlow.Application.Common.Exceptions;
using TeamFlow.Application.Common.Interfaces;

public class GetBoardQueryHandler 
    : IRequestHandler<GetBoardQuery, BoardDto>
{
    private readonly ITaskRepository _taskRepo;
    private readonly IProjectRepository _projectRepo;
    private readonly ICurrentUserService _currentUser;

    public GetBoardQueryHandler(
        ITaskRepository taskRepo,
        IProjectRepository projectRepo,
        ICurrentUserService currentUser)
    {
        _taskRepo = taskRepo;
        _projectRepo = projectRepo;
        _currentUser = currentUser;
    }

    public async Task<BoardDto> Handle(
        GetBoardQuery request,
        CancellationToken cancellationToken)
    {
        var project = await _projectRepo.GetByIdAsync(request.ProjectId)
            ?? throw new NotFoundException("Project not found");

        if (project.OwnerId != _currentUser.UserId)
            throw new UnauthorizedException("Access denied");

        var tasks = await _taskRepo.GetByProjectAsync(request.ProjectId);

        return new BoardDto(
            tasks.Where(t => t.Status == TeamFlow.Domain.Enums.TaskStatus.Todo).ToList(),
            tasks.Where(t => t.Status == TeamFlow.Domain.Enums.TaskStatus.InProgress).ToList(),
            tasks.Where(t => t.Status == TeamFlow.Domain.Enums.TaskStatus.Done).ToList()
        );
    }
}
