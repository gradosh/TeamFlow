using MediatR;

namespace TeamFlow.Application.Features.Auth.Login;

public record LoginCommand(string Email, string Password) : IRequest<LoginResult>;
public record LoginResult(string AccessToken, string RefreshToken);