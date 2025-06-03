using Application.Features.Projects.Queries; // Para GetProjectsListQuery y ProjectDto
using MediatR; // Para usar IMediator
using Microsoft.AspNetCore.Mvc;
using Application;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")] // La ruta será /api/Projects
public class ProjectsController : ControllerBase
{
    private readonly IMediator _mediator; // Inyectamos IMediator

    public ProjectsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    // Puedes añadir [Authorize] aquí cuando implementemos la autenticación
    public async Task<ActionResult<List<ProjectDto>>> GetProjects()
    {
        // Enviamos la Query a MediatR para que sea manejada por GetProjectsListQueryHandler
        var projects = await _mediator.Send(new GetProjectsListQuery());
        return Ok(projects);
    }
}