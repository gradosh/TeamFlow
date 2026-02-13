using MediatR;
using TeamFlow.Application.Common.Exceptions;
using TeamFlow.Application.Common.Interfaces;

public class GetBoardQueryHandler 
    : IRequestHandler<GetBoardQuery, BoardDto>
{
     private readonly ITaskRepository _taskRepo;
    private readonly IProjectRepository _projectRepo;
    private readonly ICurrentUserService _currentUser;
    private readonly ICacheService _cache;

    public GetBoardQueryHandler(
        ITaskRepository taskRepo,
        IProjectRepository projectRepo,
        ICurrentUserService currentUser,
        ICacheService cache)
    {
        _taskRepo = taskRepo;
        _projectRepo = projectRepo;
        _currentUser = currentUser;
        _cache = cache;
    }

    public async Task<BoardDto> Handle(
        GetBoardQuery request,
        CancellationToken cancellationToken)
    {
        var cacheKey = $"board:{_currentUser.UserId}:{request.ProjectId}";

        var cached = await _cache.GetAsync<BoardDto>(cacheKey);
        if (cached is not null)
            return cached;
            
        var project = await _projectRepo.GetByIdAsync(request.ProjectId)
            ?? throw new NotFoundException("Project not found");

        if (project.OwnerId != _currentUser.UserId)
            throw new UnauthorizedException("Access denied");

        var tasks = await _taskRepo.GetByProjectAsync(request.ProjectId);

        var boardDto = new BoardDto(
            tasks.Where(t => t.Status == TeamFlow.Domain.Enums.TaskStatus.Todo).ToList(),
            tasks.Where(t => t.Status == TeamFlow.Domain.Enums.TaskStatus.InProgress).ToList(),
            tasks.Where(t => t.Status == TeamFlow.Domain.Enums.TaskStatus.Done).ToList());
            
        await _cache.SetAsync(cacheKey, boardDto, TimeSpan.FromMinutes(5));
        
        return boardDto;
    }
}
