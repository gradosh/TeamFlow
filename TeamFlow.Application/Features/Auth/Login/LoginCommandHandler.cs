using MediatR;
using TeamFlow.Application.Common.Exceptions;
using TeamFlow.Application.Common.Interfaces;
using TeamFlow.Application.Features.Auth.Login;
using TeamFlow.Domain.Entities;

public class LoginCommandHandler
    : IRequestHandler<LoginCommand, LoginResult>
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtService _jwtService;
    private readonly IRefreshTokenRepository _refreshRepository;

    public LoginCommandHandler(
        IUserRepository userRepository,
        IJwtService jwtService,
        IRefreshTokenRepository refreshRepository)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
        _refreshRepository = refreshRepository;
    }

    public async Task<LoginResult> Handle(
        LoginCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email)
            ?? throw new UnauthorizedException("Invalid credentials");

        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            throw new UnauthorizedException("Invalid credentials");

        var accessToken = _jwtService.GenerateAccessToken(user);
        var refreshTokenValue = _jwtService.GenerateRefreshToken();

        var refreshToken = new RefreshToken(
            user.Id,
            refreshTokenValue,
            DateTime.UtcNow.AddDays(7));

        await _refreshRepository.AddAsync(refreshToken, cancellationToken);
        await _refreshRepository.SaveChangesAsync(cancellationToken);

        return new LoginResult(accessToken, refreshTokenValue);
    }
}
