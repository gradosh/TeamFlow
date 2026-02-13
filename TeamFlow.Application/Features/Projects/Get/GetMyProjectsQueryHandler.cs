using MediatR;
using TeamFlow.Application.Common.Interfaces;
using TeamFlow.Domain.Entities;

public class GetMyProjectsQueryHandler
    : IRequestHandler<GetMyProjectsQuery, List<Project>>
{
    private readonly IProjectRepository _repository;
    private readonly ICurrentUserService _currentUser;

    public GetMyProjectsQueryHandler(
        IProjectRepository repository,
        ICurrentUserService currentUser)
    {
        _repository = repository;
        _currentUser = currentUser;
    }

    public async Task<List<Project>> Handle(
        GetMyProjectsQuery request,
        CancellationToken cancellationToken)
    {
        return await _repository.GetByOwnerAsync(_currentUser.UserId);
    }
}
