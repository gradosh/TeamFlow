public interface ICacheableQuery
{
    string CacheKey { get; }
    TimeSpan Expiration { get; }
}