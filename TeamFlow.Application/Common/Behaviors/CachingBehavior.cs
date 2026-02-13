using MediatR;
using TeamFlow.Application.Common.Interfaces;

public class CachingBehavior<TRequest, TResponse> 
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ICacheService _cache;

    public CachingBehavior(ICacheService cache)
    {
        _cache = cache;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (request is not ICacheableQuery cacheableQuery)
            return await next();

        var cached = await _cache.GetAsync<TResponse>(cacheableQuery.CacheKey);

        if (cached is not null)
        {
            Console.WriteLine(" FROM CACHE (PIPELINE)");
            return cached;
        }

        Console.WriteLine(" FROM DATABASE (PIPELINE)");

        var response = await next();

        await _cache.SetAsync(
            cacheableQuery.CacheKey,
            response,
            cacheableQuery.Expiration);

        return response;
    }
}
