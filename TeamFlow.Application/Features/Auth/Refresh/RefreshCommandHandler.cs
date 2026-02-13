using MediatR;
using TeamFlow.Application.Common.Exceptions;
using TeamFlow.Application.Common.Interfaces;
using TeamFlow.Application.Features.Auth.Login;
using TeamFlow.Domain.Entities;

namespace TeamFlow.Application.Features.Auth.Refresh;

public class RefreshCommandHandler 
    : IRequestHandler<RefreshCommand, LoginResult>
{
    private readonly IRefreshTokenRepository _refreshRepository;
    private readonly IUserRepository _userRepository;
    private readonly IJwtService _jwtService;

    public RefreshCommandHandler(
        IRefreshTokenRepository refreshRepository,
        IUserRepository userRepository,
        IJwtService jwtService)
    {
        _refreshRepository = refreshRepository;
        _userRepository = userRepository;
        _jwtService = jwtService;
    }

    public async Task<LoginResult> Handle(
        RefreshCommand request,
        CancellationToken cancellationToken)
    {
        var token = await _refreshRepository.GetByTokenAsync(request.RefreshToken)
            ?? throw new UnauthorizedException("Invalid refresh token");

        if (token.IsRevoked || token.IsExpired())
            throw new UnauthorizedException("Refresh token invalid");

        var user = await _userRepository.GetByIdAsync(token.UserId)
            ?? throw new UnauthorizedException("User not found");

        // 🔥 ROTATION
        token.Revoke();

        var newRefreshValue = _jwtService.GenerateRefreshToken();
        var newRefreshToken = new RefreshToken(
            user.Id,
            newRefreshValue,
            DateTime.UtcNow.AddDays(7));

        await _refreshRepository.AddAsync(newRefreshToken, cancellationToken);
        await _refreshRepository.SaveChangesAsync(cancellationToken);

        var newAccess = _jwtService.GenerateAccessToken(user);

        return new LoginResult(newAccess, newRefreshValue);
    }
}
