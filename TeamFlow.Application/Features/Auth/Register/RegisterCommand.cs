using MediatR;

namespace TeamFlow.Application.Features.Auth.Register;

public record RegisterCommand(string Email, string Password) : IRequest<Guid>;
