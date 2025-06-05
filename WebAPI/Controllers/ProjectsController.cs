using Application.Features.Projects.Command.AddProjectMember;
using Application.Features.Projects.Command.CreateProject;
using Application.Features.Projects.Command.UpdateProject;
using Application.Features.Projects.Commands.ChangeProjectStatus;
using Application.Features.Projects.Queries;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;
namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjectsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProjectsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // GET: api/projects
    [HttpGet]
    public async Task<ActionResult<List<ProjectDto>>> GetProjects()
    {
        var projects = await _mediator.Send(new GetProjectsListQuery());
        return Ok(projects);
    }

    // POST: api/projects
    [HttpPost]
    public async Task<IActionResult> CreateProject([FromBody] CreateProjectCommand command)
    {
        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetProjectById), new { id }, null);
    }

    // GET: api/projects/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<ProjectDto>> GetProjectById(Guid id)
    {
        var project = await _mediator.Send(new GetProjectByIdQuery(id));
        return Ok(project);
    }

    // PUT: api/projects/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProject(Guid id, [FromBody] UpdateProjectCommand command)
    {
        if (id != command.Id) return BadRequest();
        var result = await _mediator.Send(command);
        return result ? NoContent() : NotFound();
    }

    // POST: api/projects/{projectId}/members
    [HttpPost("{projectId}/members")]
    public async Task<IActionResult> AddMember(Guid projectId, [FromBody] Guid userId)
    {
        var result = await _mediator.Send(new AddProjectMemberCommand(projectId, userId));
        return result ? NoContent() : NotFound();
    }
    [HttpPatch("{projectId}/status")]
    public async Task<IActionResult> ChangeStatus(Guid projectId, [FromBody] ProjectStatus newStatus)
    {
        var result = await _mediator.Send(new ChangeProjectStatusCommand(projectId, newStatus));
        return result ? NoContent() : NotFound();
    }

}
