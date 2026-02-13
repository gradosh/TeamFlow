using MediatR;
using TeamFlow.Application.Common.Interfaces;
using TeamFlow.Domain.Entities;

namespace TeamFlow.Application.Features.Auth.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Guid>
{
    private readonly IUserRepository _userRepository;

    public RegisterCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Guid> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var existing = await _userRepository.GetByEmailAsync(request.Email);
        if (existing != null)
            throw new Exception("User already exists");

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

        var user = new User(request.Email, passwordHash);

        await _userRepository.AddAsync(user);

        return user.Id;
    }
}
