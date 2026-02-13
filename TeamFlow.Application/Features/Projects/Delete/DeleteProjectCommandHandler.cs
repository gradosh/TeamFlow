using MediatR;
using TeamFlow.Application.Common.Exceptions;
using TeamFlow.Application.Common.Interfaces;

public class DeleteProjectCommandHandler
    : IRequestHandler<DeleteProjectCommand>
{
    private readonly IProjectRepository _repository;
    private readonly ICurrentUserService _currentUser;

    public DeleteProjectCommandHandler(
        IProjectRepository repository,
        ICurrentUserService currentUser)
    {
        _repository = repository;
        _currentUser = currentUser;
    }

    public async Task Handle(
        DeleteProjectCommand request,
        CancellationToken cancellationToken)
    {
        var project = await _repository.GetByIdAsync(request.ProjectId)
            ?? throw new NotFoundException("Project not found");

        if (project.OwnerId != _currentUser.UserId)
            throw new UnauthorizedException("Access denied");

        await _repository.RemoveAsync(project);
        await _repository.SaveChangesAsync(cancellationToken);
    }
}
