using MediatR;

public record GetBoardQuery(Guid ProjectId, Guid UserId)
    : IRequest<BoardDto>, ICacheableQuery
{
    public string CacheKey => $"board:{UserId}:{ProjectId}";
    public TimeSpan Expiration => TimeSpan.FromMinutes(5);
}
