using System.Security.Claims;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeamFlow.Contracts.Tasks;

[Authorize]
[Route("api/tasks")]
public class TasksController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public TasksController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTaskRequest request){
        var command = _mapper.Map<CreateTaskCommand>(request);
        var taskId = await _mediator.Send(command);
        return Ok(new CreateTaskResponse(taskId,request.ProjectId,request.Title,request.Description,DateTime.UtcNow));        
    }

    [HttpPatch("status")]
    public async Task<IActionResult> UpdateStatus([FromBody] UpdateTaskStatusRequest request )
    {
        var command = _mapper.Map<UpdateTaskStatusCommand>(request);
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpGet("board/{projectId}")]
    public async Task<IActionResult> GetBoard(Guid projectId)
    {
        var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        var result = await _mediator.Send(new GetBoardQuery(projectId, Guid.Parse(userId!)));

        var response = _mapper.Map<BoardResponse>(result);

        return Ok(response);
    }
}
