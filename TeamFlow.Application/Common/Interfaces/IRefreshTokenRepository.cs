using TeamFlow.Domain.Entities;

namespace TeamFlow.Application.Common.Interfaces;

public interface IRefreshTokenRepository
{
    Task AddAsync(RefreshToken token, CancellationToken cancellationToken);
    Task<RefreshToken?> GetByTokenAsync(string token);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
