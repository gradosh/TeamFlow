using MediatR;
using TeamFlow.Application.Common.Interfaces;
using TeamFlow.Application.Features.Projects.Create;
using TeamFlow.Domain.Entities;

public class CreateProjectCommandHandler 
    : IRequestHandler<CreateProjectCommand, Guid>
{
    private readonly IProjectRepository _repository;
    private readonly ICurrentUserService _currentUser;

    public CreateProjectCommandHandler(
        IProjectRepository repository,
        ICurrentUserService currentUser)
    {
        _repository = repository;
        _currentUser = currentUser;
    }

    public async Task<Guid> Handle(
        CreateProjectCommand request,
        CancellationToken cancellationToken)
    {
        var project = new Project(
            request.Name,
            request.Description,
            _currentUser.UserId);

        await _repository.AddAsync(project, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        return project.Id;
    }
}
