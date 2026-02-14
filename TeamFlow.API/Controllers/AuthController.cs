using MediatR;
using Microsoft.AspNetCore.Mvc;
using TeamFlow.Application.Features.Auth.Login;
using TeamFlow.Application.Features.Auth.Register;

namespace TeamFlow.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterCommand command)
    {
        var id = await _mediator.Send(command);
        return Ok(id);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}
