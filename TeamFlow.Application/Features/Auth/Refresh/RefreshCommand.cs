using MediatR;
using TeamFlow.Application.Features.Auth.Login;

namespace TeamFlow.Application.Features.Auth.Refresh;

public record RefreshCommand(string RefreshToken) : IRequest<LoginResult>;
