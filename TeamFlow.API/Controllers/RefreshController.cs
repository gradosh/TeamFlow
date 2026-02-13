using MediatR;
using Microsoft.AspNetCore.Mvc;
using TeamFlow.Application.Features.Auth.Login;
using TeamFlow.Application.Features.Auth.Refresh;

[ApiController]
[Route("api/refresh")]
public class RefreshController : ControllerBase
{
    private readonly IMediator _mediator;

    public RefreshController(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh(RefreshCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}