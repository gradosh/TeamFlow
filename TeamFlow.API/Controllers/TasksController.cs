using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
[Route("api/tasks")]
public class TasksController : ControllerBase
{
    private readonly IMediator _mediator;

    public TasksController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateTaskCommand command)
        => Ok(await _mediator.Send(command));

    [HttpPatch("status")]
    public async Task<IActionResult> UpdateStatus(UpdateTaskStatusCommand command)
    {
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpGet("board/{projectId}")]
    public async Task<IActionResult> GetBoard(Guid projectId)
    {
        var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Ok(await _mediator.Send(
            new GetBoardQuery(projectId, Guid.Parse(userId!))));
    }
}
