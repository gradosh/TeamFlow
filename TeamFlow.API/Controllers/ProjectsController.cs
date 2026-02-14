using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeamFlow.API.Contracts.Projects;
using TeamFlow.Application.Features.Projects.Create;

[Authorize]
[Route("api/projects")]
public class ProjectsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public ProjectsController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody]CreateProjectRequest request)
    {
        var command = _mapper.Map<CreateProjectCommand>(request);
        var id = await _mediator.Send(command);
        return Ok(new CreateProjectResponse(id, request.Name, request.Description,DateTime.UtcNow));
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var projects = await _mediator.Send(new GetMyProjectsQuery());
        return Ok(projects);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new DeleteProjectCommand(id));
        return NoContent();
    }
}
